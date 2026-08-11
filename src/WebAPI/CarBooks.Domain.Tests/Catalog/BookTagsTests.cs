using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class BookTagsTests
{
    [Fact]
    public void Constructor_ValidIds_SetsTagAndBookIds()
    {
        // Arrange
        var tagId = Guid.Parse("33333333-3333-4333-8333-333333330001");
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var link = new BookTags(tagId, bookId);

        // Assert
        Assert.Equal(tagId, link.TagId);
        Assert.Equal(bookId, link.BookId);
    }

    [Fact]
    public void Constructor_EmptyTagId_ThrowsDomainValidationException()
    {
        // Arrange
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var act = () => new BookTags(Guid.Empty, bookId);

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Fact]
    public void Constructor_EmptyBookId_ThrowsDomainValidationException()
    {
        // Arrange
        var tagId = Guid.Parse("33333333-3333-4333-8333-333333330001");

        // Act
        var act = () => new BookTags(tagId, Guid.Empty);

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }
}
