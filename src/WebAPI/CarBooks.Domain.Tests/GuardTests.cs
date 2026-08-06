using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests;

public sealed class GuardTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Text_BlankValue_ThrowsDomainValidationException(string? value)
    {
        // Arrange
        const string fieldName = "Name";

        // Act
        var exception = Assert.Throws<DomainValidationException>(() => Guard.Text(value, fieldName));

        // Assert
        Assert.Equal("Name is required.", exception.Message);
    }

    [Fact]
    public void Text_PaddedValue_ReturnsTrimmedValue()
    {
        // Arrange
        const string value = "  Go Like Hell  ";

        // Act
        var result = Guard.Text(value, "Name");

        // Assert
        Assert.Equal("Go Like Hell", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void OptionalText_BlankValue_ReturnsNull(string? value)
    {
        // Arrange

        // Act
        var result = Guard.OptionalText(value);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void OptionalText_PaddedValue_ReturnsTrimmedValue()
    {
        // Arrange
        const string value = "  Penguin  ";

        // Act
        var result = Guard.OptionalText(value);

        // Assert
        Assert.Equal("Penguin", result);
    }

    [Fact]
    public void AbsoluteUrl_HttpsUrl_ReturnsTrimmedUrl()
    {
        // Arrange
        const string value = " https://example.com/a.png ";

        // Act
        var result = Guard.AbsoluteUrl(value, "CoverUrl");

        // Assert
        Assert.Equal("https://example.com/a.png", result);
    }

    [Fact]
    public void AbsoluteUrl_HttpUrl_ReturnsUrl()
    {
        // Arrange
        const string value = "http://example.com/a.png";

        // Act
        var result = Guard.AbsoluteUrl(value, "CoverUrl");

        // Assert
        Assert.Equal("http://example.com/a.png", result);
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("ftp://example.com/a.png")]
    [InlineData("/relative/path")]
    public void AbsoluteUrl_InvalidUrl_ThrowsDomainValidationException(string value)
    {
        // Arrange & Act
        var act = () => Guard.AbsoluteUrl(value, "CoverUrl");

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void OptionalAbsoluteUrl_BlankValue_ReturnsNull(string? value)
    {
        // Arrange & Act
        var result = Guard.OptionalAbsoluteUrl(value, "CoverUrl");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void OptionalAbsoluteUrl_InvalidUrl_ThrowsDomainValidationException()
    {
        // Arrange
        const string value = "ftp://example.com";

        // Act
        var act = () => Guard.OptionalAbsoluteUrl(value, "CoverUrl");

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }
}
