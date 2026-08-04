using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Domain service coordinating category and book aggregates when a use case spans both.
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
    /// Resolves a category by its id together with its books.
    /// </summary>
    /// <exception cref="EntityNotFoundException">No category carries the requested id.</exception>
    public async Task<CategoryBooks> GetCategoryBooksAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FindAsync(categoryId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Category), categoryId);

        var books = await bookRepository.ListByCategoryAsync(category.Id, cancellationToken);
        return new CategoryBooks(category, books);
    }
}

/// <summary>A category paired with the books it contains.</summary>
public sealed record CategoryBooks(Category Category, IReadOnlyList<Book> Books);
