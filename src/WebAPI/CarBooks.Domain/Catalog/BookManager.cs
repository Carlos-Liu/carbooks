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

    public BookManager(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository,
        ICategoryBooksRepository categoryBooksRepository)
    {
        this.bookRepository = bookRepository;
        this.categoryRepository = categoryRepository;
        this.categoryBooksRepository = categoryBooksRepository;
    }

    /// <summary>
    /// Creates and persists a new book, optionally with a cover image and category assignments.
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
        IEnumerable<Guid> categoryIds,
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

        if (coverImage is { Length: > 0 })
        {
            if (string.IsNullOrWhiteSpace(coverImageContentType))
            {
                throw new DomainValidationException("Cover image content type is required when an image is uploaded.");
            }

            book.SetCoverImage(coverImage, coverImageContentType);
        }

        await bookRepository.AddAsync(book, cancellationToken);

        var distinctCategoryIds = categoryIds.Distinct().ToList();
        if (distinctCategoryIds.Count == 0)
        {
            return book;
        }

        var links = new List<CategoryBooks>(distinctCategoryIds.Count);
        foreach (var categoryId in distinctCategoryIds)
        {
            _ = await categoryRepository.FindAsync(categoryId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Category), categoryId);

            links.Add(new CategoryBooks(categoryId, book.Id));
        }

        await categoryBooksRepository.AddRangeAsync(links, cancellationToken);
        return book;
    }
}
