using CarBooks.Application.Shared.Catalog.Dtos;

namespace CarBooks.Application.Shared.Catalog;

public interface ITagAppService : IApplicationService
{
    /// <summary>Returns every tag available for assignment, ordered for presentation.</summary>
    Task<IReadOnlyList<TagDto>> GetTagsAsync(CancellationToken cancellationToken);
}
