using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarBooks.WebAPI.Controllers;

[ApiController]
[Produces("application/json")]
public sealed class BooksController : ControllerBase
{
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
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> CreateBookAsync(
        [FromForm] CreateBookDto request,
        CancellationToken cancellationToken)
    {
        var book = await bookAppService.CreateBookAsync(request, cancellationToken);
        return Created($"api/books/{book.Id}", book);
    }
}
