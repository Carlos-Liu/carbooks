using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Domain service for book-only operations that need persistence or rules beyond a single entity.
/// </summary>
public sealed class BookManager
{
    private readonly IBookRepository bookRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly ICategoryBooksRepository categoryBooksRepository;
    private readonly ITagRepository tagRepository;
    private readonly IBookTagsRepository bookTagsRepository;
    private readonly IUnitOfWork unitOfWork;

    public BookManager(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository,
        ICategoryBooksRepository categoryBooksRepository,
        ITagRepository tagRepository,
        IBookTagsRepository bookTagsRepository,
        IUnitOfWork unitOfWork)
    {
        this.bookRepository = bookRepository;
        this.categoryRepository = categoryRepository;
        this.categoryBooksRepository = categoryBooksRepository;
        this.tagRepository = tagRepository;
        this.bookTagsRepository = bookTagsRepository;
        this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates and persists a new book, optionally with a cover image, categories, and tags.
    /// All inserts are flushed in a single <see cref="IUnitOfWork.SaveChangesAsync"/> call.
    /// </summary>
    public async Task<Book> AddBookAsync(
        string name,
        string author,
        string? coverUrl,
        string? translator,
        string? publisher,
        DateOnly? publishedOn,
        string? recommendation,
        string? isbn,
        byte[]? coverImage,
        string? coverImageContentType,
        IEnumerable<Guid>? categoryIds,
        IEnumerable<Guid>? tagIds,
        CancellationToken cancellationToken)
    {
        var book = new Book(
            Entity.NewId(),
            name,
            author,
            coverUrl,
            translator,
            publisher,
            publishedOn,
            recommendation,
            isbn);

        book.SetCoverImage(coverImage, coverImageContentType);

        await bookRepository.AddAsync(book, cancellationToken);
        await AssignCategoriesAsync(book.Id, categoryIds, cancellationToken);
        await AssignTagsAsync(book.Id, tagIds, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return book;
    }

    private async Task AssignCategoriesAsync(
        Guid bookId,
        IEnumerable<Guid>? categoryIds,
        CancellationToken cancellationToken)
    {
        var distinctCategoryIds = categoryIds?.Distinct().ToList() ?? [];
        if (distinctCategoryIds.Count == 0)
        {
            return;
        }

        var links = new List<CategoryBooks>(distinctCategoryIds.Count);
        var categoryEntities = await categoryRepository.FindByIdsAsync(distinctCategoryIds, cancellationToken);

        foreach (var categoryId in distinctCategoryIds)
        {
            var categoryExist = categoryEntities.Any(c => c.Id == categoryId);
            if (!categoryExist)
            {
                throw new EntityNotFoundException(nameof(Category), categoryId);
            }

            links.Add(new CategoryBooks(categoryId, bookId));
        }

        await categoryBooksRepository.AddRangeAsync(links, cancellationToken);
    }

    private async Task AssignTagsAsync(
        Guid bookId,
        IEnumerable<Guid>? tagIds,
        CancellationToken cancellationToken)
    {
        var distinctTagIds = tagIds?.Distinct().ToList() ?? [];
        if (distinctTagIds.Count == 0)
        {
            return;
        }

        var links = new List<BookTags>(distinctTagIds.Count);        
        var tagEntities = await tagRepository.FindByIdsAsync(distinctTagIds, cancellationToken);

        foreach (var tagId in distinctTagIds)
        {
            var tagExist = tagEntities.Any(t => t.Id == tagId);
            if (!tagExist)
            {
                throw new EntityNotFoundException(nameof(Tag), tagId);
            }

            links.Add(new BookTags(tagId, bookId));
        }

        await bookTagsRepository.AddRangeAsync(links, cancellationToken);
    }
}
