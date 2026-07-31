using CarBooks.Application.Shared.Catalog.Dtos;

namespace CarBooks.Application.Shared.Catalog;

public interface ICategoryAppService : IApplicationService
{
    /// <summary>Returns every category for the main page, ordered for presentation.</summary>
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);
}
