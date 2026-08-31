using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CarBooks.WebAPI.Controllers;

[ApiController]
[Produces("application/json")]
public sealed class BooksController : ControllerBase
{
    private const int MultipartFormOverheadLimitBytes = 64 * 1024; // 64 KB for form data overhead
    private const int MultipartRequestLimitBytes = Consts.MaxCoverImageBytes + MultipartFormOverheadLimitBytes;

    private readonly IBookAppService bookAppService;

    public BooksController(IBookAppService bookAppService)
    {
        this.bookAppService = bookAppService;
    }

    /// <summary>Returns a category together with the books it contains.</summary>
    /// <param name="categoryId">Category identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("api/categories/{categoryId:guid}/books")]
    [ProducesResponseType(typeof(CategoryBooksDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public Task<CategoryBooksDto> GetBooksAsync(Guid categoryId, CancellationToken cancellationToken) =>
        bookAppService.GetBooksByCategoryIdAsync(categoryId, cancellationToken);

    /// <summary>
    /// Creates a book. Send <c>multipart/form-data</c> with text fields and an optional local
    /// cover image file.
    /// </summary>
    /// <param name="request">Book fields and optional cover image upload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("api/books")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(MultipartRequestLimitBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = MultipartRequestLimitBytes)]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> CreateBookAsync(
        [FromForm] CreateBookDto request,
        [FromForm] IFormFile? coverImage,
        CancellationToken cancellationToken)
    {
        byte[]? coverImageContent = null;
        if (coverImage is { Length: > 0 })
        {
            if (coverImage.Length > Consts.MaxCoverImageBytes)
            {
                throw new DomainValidationException($"Cover image must be {Consts.MaxCoverImageBytes} bytes or fewer.");
            }

            await using var stream = coverImage.OpenReadStream();
            coverImageContent = new byte[coverImage.Length];
            await stream.ReadExactlyAsync(coverImageContent, cancellationToken);
        }

        var coverImageDto = coverImageContent is null ? null : new CoverImageDto
        {
            Content = coverImageContent,
            ContentType = coverImage?.ContentType,
        };

        var book = await bookAppService.CreateBookAsync(request, coverImageDto, cancellationToken);
        return Created($"api/books/{book.Id}", book);
    }
}
