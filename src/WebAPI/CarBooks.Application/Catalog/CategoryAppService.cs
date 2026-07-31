using CarBooks.Application.Catalog.Mapping;
using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Catalog;
using Microsoft.Extensions.Logging;

namespace CarBooks.Application.Catalog;

internal sealed class CategoryAppService : ICategoryAppService
{
    private readonly CatalogManager catalogManager;
    private readonly ILogger<CategoryAppService> logger;

    public CategoryAppService(CatalogManager catalogManager, ILogger<CategoryAppService> logger)
    {
        this.catalogManager = catalogManager;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = await catalogManager.GetCategoriesAsync(cancellationToken);
        var bookCounts = await catalogManager.GetBookCountsAsync(cancellationToken);

        logger.LogInformation("Returning {CategoryCount} catalog categories.", categories.Count);

        return categories
            .Select(category => category.ToDto(bookCounts.GetValueOrDefault(category.Id)))
            .ToList();
    }
}
