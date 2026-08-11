using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarBooks.WebAPI.Controllers;

[ApiController]
[Route("api/tags")]
[Produces("application/json")]
public sealed class TagsController : ControllerBase
{
    private readonly ITagAppService tagAppService;

    public TagsController(ITagAppService tagAppService)
    {
        this.tagAppService = tagAppService;
    }

    /// <summary>Returns tags available when creating or labeling books.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TagDto>), StatusCodes.Status200OK)]
    public Task<IReadOnlyList<TagDto>> GetTagsAsync(CancellationToken cancellationToken) =>
        tagAppService.GetTagsAsync(cancellationToken);
}
