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

    public async Task<CategoryBooksDto> GetBooksByCategorySlugAsync(
        string categorySlug,
        CancellationToken cancellationToken)
    {
        var result = await catalogManager.GetCategoryBooksAsync(categorySlug, cancellationToken);

        logger.LogInformation(
            "Returning {BookCount} books for category {CategorySlug}.",
            result.Books.Count,
            result.Category.Slug);

        return new CategoryBooksDto(
            result.Category.ToDto(result.Books.Count),
            result.Books.ToDtos(dataUriFactory));
    }
}
