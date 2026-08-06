using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class CategoryBooksTests
{
    [Fact]
    public void Constructor_ValidIds_SetsCategoryAndBookIds()
    {
        // Arrange
        var categoryId = Guid.Parse("11111111-1111-4111-8111-111111110001");
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var link = new CategoryBooks(categoryId, bookId);

        // Assert
        Assert.Equal(categoryId, link.CategoryId);
        Assert.Equal(bookId, link.BookId);
    }

    [Fact]
    public void Constructor_EmptyCategoryId_ThrowsDomainValidationException()
    {
        // Arrange
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var act = () => new CategoryBooks(Guid.Empty, bookId);

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Fact]
    public void Constructor_EmptyBookId_ThrowsDomainValidationException()
    {
        // Arrange
        var categoryId = Guid.Parse("11111111-1111-4111-8111-111111110001");

        // Act
        var act = () => new CategoryBooks(categoryId, Guid.Empty);

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }
}
