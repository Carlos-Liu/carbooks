using CarBooks.Application.Shared.Catalog.Dtos;

namespace CarBooks.Application.Shared.Catalog;

public interface IBookAppService : IApplicationService
{
    /// <summary>Returns a category together with its books.</summary>
    /// <param name="categoryId">The ID of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="Domain.Shared.Errors.EntityNotFoundException">The id matches no category.</exception>
    Task<CategoryBooksDto> GetBooksByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    /// <summary>Returns a tag together with the books labeled by it.</summary>
    /// <param name="tagId">The ID of the tag.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="Domain.Shared.Errors.EntityNotFoundException">The id matches no tag.</exception>
    Task<TagBooksDto> GetBooksByTagIdAsync(Guid tagId, CancellationToken cancellationToken);

    /// <summary>Creates a book from the provided fields and optional cover image payload.</summary>
    /// <param name="request">The book creation request.</param>
    /// <param name="coverImage">The optional cover image payload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="Domain.Shared.Errors.DomainValidationException">The payload is invalid.</exception>
    Task<BookDto> CreateBookAsync(
        CreateBookDto request,
        CoverImageDto? coverImage,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the cover image thumbnail for a book by its ID.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The cover image thumbnail.</returns>
    Task<CoverImageDto> GetCoverThumbnailAsync(Guid bookId, CancellationToken cancellationToken);
}
