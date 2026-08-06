using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class CatalogManagerTests
{
    private readonly ICategoryRepository categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IBookRepository bookRepository = Substitute.For<IBookRepository>();
    private readonly CatalogManager catalogManager;

    public CatalogManagerTests()
    {
        catalogManager = new CatalogManager(categoryRepository, bookRepository);
    }

    [Fact]
    public async Task GetCategoryBooksAsync_ExistingCategory_ReturnsCategoryAndBooks()
    {
        // Arrange
        var categoryId = Guid.Parse("11111111-1111-4111-8111-111111110001");
        var category = new Category(categoryId, "Category 1");
        var books = new List<Book>
        {
            new(Guid.Parse("22222222-2222-4222-8222-222222220001"), "Go Like Hell", "A. J. Baime"),
        };
        categoryRepository.FindAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
        bookRepository.ListByCategoryAsync(categoryId, Arg.Any<CancellationToken>()).Returns(books);

        // Act
        var result = await catalogManager.GetCategoryBooksAsync(categoryId, CancellationToken.None);

        // Assert
        Assert.Same(category, result.Category);
        Assert.Same(books, result.Books);
    }

    [Fact]
    public async Task GetCategoryBooksAsync_MissingCategory_ThrowsEntityNotFoundException()
    {
        // Arrange
        var categoryId = Guid.Parse("11111111-1111-4111-8111-111111110099");
        categoryRepository.FindAsync(categoryId, Arg.Any<CancellationToken>()).Returns((Category?)null);

        // Act
        var act = () => catalogManager.GetCategoryBooksAsync(categoryId, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(act);
    }

    [Fact]
    public async Task GetCategoriesAsync_CategoriesExist_ReturnsRepositoryResult()
    {
        // Arrange
        var categories = new List<Category>
        {
            new(Guid.Parse("11111111-1111-4111-8111-111111110001"), "Category 1"),
        };
        categoryRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(categories);

        // Act
        var result = await catalogManager.GetCategoriesAsync(CancellationToken.None);

        // Assert
        Assert.Same(categories, result);
    }

    [Fact]
    public async Task GetBookCountsAsync_CountsExist_ReturnsRepositoryResult()
    {
        // Arrange
        var counts = new Dictionary<Guid, int>
        {
            [Guid.Parse("11111111-1111-4111-8111-111111110001")] = 2,
        };
        categoryRepository.CountBooksByCategoryAsync(Arg.Any<CancellationToken>()).Returns(counts);

        // Act
        var result = await catalogManager.GetBookCountsAsync(CancellationToken.None);

        // Assert
        Assert.Same(counts, result);
    }
}
