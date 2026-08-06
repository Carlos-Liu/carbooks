using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class BookTests
{
    [Fact]
    public void Constructor_AllFieldsProvided_SetsTrimmedValues()
    {
        // Arrange
        var id = Guid.Parse("22222222-2222-4222-8222-222222220001");
        var publishedOn = new DateOnly(2020, 1, 15);

        // Act
        var book = new Book(
            id,
            "  Go Like Hell  ",
            "  A. J. Baime  ",
            " https://example.com/cover.png ",
            "  Translator  ",
            "  Pub  ",
            publishedOn,
            "  Great read  ",
            "  978-1-234  ");

        // Assert
        Assert.Equal("Go Like Hell", book.Name);
        Assert.Equal("A. J. Baime", book.Author);
        Assert.Equal("https://example.com/cover.png", book.CoverUrl);
        Assert.Equal("Translator", book.Translator);
        Assert.Equal("Pub", book.Publisher);
        Assert.Equal(publishedOn, book.PublishedOn);
        Assert.Equal("Great read", book.Recommendation);
        Assert.Equal("978-1-234", book.Isbn);
        Assert.False(book.HasCoverImage);
    }

    [Fact]
    public void Constructor_OptionalFieldsOmitted_SetsNullOptionalFields()
    {
        // Arrange
        var id = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var book = new Book(id, "Go Like Hell", "A. J. Baime");

        // Assert
        Assert.Null(book.CoverUrl);
        Assert.Null(book.Translator);
        Assert.Null(book.Publisher);
        Assert.Null(book.PublishedOn);
        Assert.Null(book.Recommendation);
        Assert.Null(book.Isbn);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_BlankName_ThrowsDomainValidationException(string? name)
    {
        // Arrange
        var id = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var act = () => new Book(id, name!, "Author");

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Fact]
    public void Constructor_InvalidCoverUrl_ThrowsDomainValidationException()
    {
        // Arrange
        var id = Guid.Parse("22222222-2222-4222-8222-222222220001");

        // Act
        var act = () => new Book(id, "Go Like Hell", "A. J. Baime", "not-a-url");

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Fact]
    public void SetCoverImage_ValidImage_StoresImageAndSetsHasCoverImage()
    {
        // Arrange
        var book = new Book(
            Guid.Parse("22222222-2222-4222-8222-222222220001"),
            "Go Like Hell",
            "A. J. Baime");
        byte[] content = [1, 2, 3];

        // Act
        book.SetCoverImage(content, "image/png");

        // Assert
        Assert.True(book.HasCoverImage);
        Assert.Equal(content, book.CoverImage);
        Assert.Equal("image/png", book.CoverImageContentType);
    }

    [Fact]
    public void SetCoverImage_EmptyContent_ThrowsDomainValidationException()
    {
        // Arrange
        var book = new Book(
            Guid.Parse("22222222-2222-4222-8222-222222220001"),
            "Go Like Hell",
            "A. J. Baime");

        // Act
        var act = () => book.SetCoverImage([], "image/png");

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Fact]
    public void SetCoverImage_BlankContentType_ThrowsDomainValidationException()
    {
        // Arrange
        var book = new Book(
            Guid.Parse("22222222-2222-4222-8222-222222220001"),
            "Go Like Hell",
            "A. J. Baime");

        // Act
        var act = () => book.SetCoverImage([1], "  ");

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Fact]
    public void ClearCoverImage_ExistingImage_RemovesImage()
    {
        // Arrange
        var book = new Book(
            Guid.Parse("22222222-2222-4222-8222-222222220001"),
            "Go Like Hell",
            "A. J. Baime");
        book.SetCoverImage([1], "image/png");

        // Act
        book.ClearCoverImage();

        // Assert
        Assert.False(book.HasCoverImage);
        Assert.Null(book.CoverImage);
        Assert.Null(book.CoverImageContentType);
    }
}
