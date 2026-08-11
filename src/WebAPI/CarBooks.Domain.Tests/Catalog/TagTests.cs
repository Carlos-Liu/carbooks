using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class TagTests
{
    [Fact]
    public void Constructor_PaddedName_SetsTrimmedName()
    {
        // Arrange
        var id = Guid.Parse("33333333-3333-4333-8333-333333330001");
        const string name = "  Racing  ";

        // Act
        var tag = new Tag(id, name);

        // Assert
        Assert.Equal("Racing", tag.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_BlankName_ThrowsDomainValidationException(string? name)
    {
        // Arrange
        var id = Guid.Parse("33333333-3333-4333-8333-333333330001");

        // Act
        var act = () => new Tag(id, name!);

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }
}
