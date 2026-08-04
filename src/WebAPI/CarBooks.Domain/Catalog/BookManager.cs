using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Domain service for book-only operations that need persistence or rules beyond a single entity.
/// </summary>
public sealed class BookManager
{
    private readonly IBookRepository bookRepository;

    public BookManager(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    /// <summary>Creates and persists a new book, optionally with a locally uploaded cover image.</summary>
    public async Task<Book> AddBookAsync(
        string name,
        string author,
        string coverUrl,
        byte[]? coverImage,
        string? coverImageContentType,
        CancellationToken cancellationToken)
    {
        var book = new Book(Entity.NewId(), name, author, coverUrl);

        if (coverImage is { Length: > 0 })
        {
            if (string.IsNullOrWhiteSpace(coverImageContentType))
            {
                throw new DomainValidationException("Cover image content type is required when an image is uploaded.");
            }

            book.SetCoverImage(coverImage, coverImageContentType);
        }

        await bookRepository.AddAsync(book, cancellationToken);
        return book;
    }
}
