using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Domain service coordinating the category and book aggregates. It owns the rules that span both
/// of them, keeping the application layer free of business decisions.
/// </summary>
public sealed class CatalogManager
{
    private readonly ICategoryRepository categoryRepository;
    private readonly IBookRepository bookRepository;

    public CatalogManager(ICategoryRepository categoryRepository, IBookRepository bookRepository)
    {
        this.categoryRepository = categoryRepository;
        this.bookRepository = bookRepository;
    }

    public Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken) =>
        categoryRepository.ListAsync(cancellationToken);

    public Task<IReadOnlyDictionary<Guid, int>> GetBookCountsAsync(CancellationToken cancellationToken) =>
        categoryRepository.CountBooksByCategoryAsync(cancellationToken);

    /// <summary>
    /// Resolves a category by its slug together with its books.
    /// </summary>
    /// <exception cref="EntityNotFoundException">No category carries the requested slug.</exception>
    public async Task<CategoryBooks> GetCategoryBooksAsync(string slug, CancellationToken cancellationToken)
    {
        var normalised = Guard.Text(slug, "Category slug", Shared.CatalogConsts.MaxCategorySlugLength).ToLowerInvariant();

        var category = await categoryRepository.FindBySlugAsync(normalised, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Category), normalised);

        var books = await bookRepository.ListByCategoryAsync(category.Id, cancellationToken);
        return new CategoryBooks(category, books);
    }
}

/// <summary>A category paired with the books it contains.</summary>
public sealed record CategoryBooks(Category Category, IReadOnlyList<Book> Books);
