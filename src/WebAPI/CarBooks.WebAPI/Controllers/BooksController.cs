using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarBooks.WebAPI.Controllers;

[ApiController]
[Route("api/categories/{categorySlug}/books")]
[Produces("application/json")]
public sealed class BooksController : ControllerBase
{
    private readonly IBookAppService bookAppService;

    public BooksController(IBookAppService bookAppService)
    {
        this.bookAppService = bookAppService;
    }

    /// <summary>Returns a category together with the books it contains.</summary>
    /// <param name="categorySlug">URL-safe category identifier, for example <c>category-1</c>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(CategoryBooksDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public Task<CategoryBooksDto> GetBooksAsync(string categorySlug, CancellationToken cancellationToken) =>
        bookAppService.GetBooksByCategorySlugAsync(categorySlug, cancellationToken);
}
