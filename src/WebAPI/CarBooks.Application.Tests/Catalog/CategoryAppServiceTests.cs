using CarBooks.Application.Catalog;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace CarBooks.Application.Tests.Catalog;

public sealed class CategoryAppServiceTests
{
    private readonly ICategoryRepository categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IBookRepository bookRepository = Substitute.For<IBookRepository>();
    private readonly CategoryAppService categoryAppService;

    public CategoryAppServiceTests()
    {
        var catalogManager = new CatalogManager(categoryRepository, bookRepository);
        categoryAppService = new CategoryAppService(catalogManager, NullLogger<CategoryAppService>.Instance);
    }

    [Fact]
    public async Task GetCategoriesAsync_CategoriesWithCounts_ReturnsMappedBookCounts()
    {
        // Arrange
        var categoryWithBooks = Guid.Parse("11111111-1111-4111-8111-111111110001");
        var categoryWithoutBooks = Guid.Parse("11111111-1111-4111-8111-111111110002");
        categoryRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(
        [
            new Category(categoryWithBooks, "Category 1"),
            new Category(categoryWithoutBooks, "Category 2"),
        ]);
        categoryRepository.CountBooksByCategoryAsync(Arg.Any<CancellationToken>()).Returns(
            new Dictionary<Guid, int> { [categoryWithBooks] = 3 });

        // Act
        var result = await categoryAppService.GetCategoriesAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(3, result[0].BookCount);
        Assert.Equal(0, result[1].BookCount);
        Assert.Equal("Category 1", result[0].Name);
        Assert.Equal("Category 2", result[1].Name);
    }

    [Fact]
    public async Task GetCategoriesAsync_EmptyCatalog_ReturnsEmptyList()
    {
        // Arrange
        categoryRepository.ListAsync(Arg.Any<CancellationToken>()).Returns([]);
        categoryRepository.CountBooksByCategoryAsync(Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, int>());

        // Act
        var result = await categoryAppService.GetCategoriesAsync(CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
