using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Domain service coordinating category, tag, and book aggregates when a use case spans them.
/// </summary>
public sealed class CatalogManager
{
    private readonly ICategoryRepository categoryRepository;
    private readonly ITagRepository tagRepository;
    private readonly IBookRepository bookRepository;

    public CatalogManager(
        ICategoryRepository categoryRepository,
        ITagRepository tagRepository,
        IBookRepository bookRepository)
    {
        this.categoryRepository = categoryRepository;
        this.tagRepository = tagRepository;
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
    public async Task<CategoryWithBooks> GetCategoryBooksAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FindAsync(categoryId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Category), categoryId);

        var books = await bookRepository.ListByCategoryAsync(category.Id, cancellationToken);
        return new CategoryWithBooks(category, books);
    }

    /// <summary>
    /// Resolves a tag by its id together with the books labeled by it.
    /// </summary>
    /// <exception cref="EntityNotFoundException">No tag carries the requested id.</exception>
    public async Task<TagWithBooks> GetTagBooksAsync(Guid tagId, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.FindAsync(tagId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Tag), tagId);

        var books = await bookRepository.ListByTagAsync(tag.Id, cancellationToken);
        return new TagWithBooks(tag, books);
    }
}
