using CarBooks.Application.Catalog.Mapping;
using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Catalog;
using CarBooks.Infrastructure.Media;
using Microsoft.Extensions.Logging;

namespace CarBooks.Application.Catalog;

internal sealed class BookAppService : IBookAppService
{
    private readonly CatalogManager catalogManager;
    private readonly IDataUriFactory dataUriFactory;
    private readonly ILogger<BookAppService> logger;

    public BookAppService(
        CatalogManager catalogManager,
        IDataUriFactory dataUriFactory,
        ILogger<BookAppService> logger)
    {
        this.catalogManager = catalogManager;
        this.dataUriFactory = dataUriFactory;
        this.logger = logger;
    }

    public async Task<CategoryBooksDto> GetBooksByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var result = await catalogManager.GetCategoryBooksAsync(categoryId, cancellationToken);

        logger.LogInformation(
            "Returning {BookCount} books for category {CategoryId}.",
            result.Books.Count,
            result.Category.Id);

        return new CategoryBooksDto(
            result.Category.ToDto(result.Books.Count),
            result.Books.ToDtos(dataUriFactory));
    }
}
