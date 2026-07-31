using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarBooks.WebAPI.Controllers;

[ApiController]
[Route("api/categories")]
[Produces("application/json")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICategoryAppService categoryAppService;

    public CategoriesController(ICategoryAppService categoryAppService)
    {
        this.categoryAppService = categoryAppService;
    }

    /// <summary>Returns the categories listed on the main page.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
    public Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken) =>
        categoryAppService.GetCategoriesAsync(cancellationToken);
}
