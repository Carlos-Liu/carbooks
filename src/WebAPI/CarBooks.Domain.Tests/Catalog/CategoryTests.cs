using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class CategoryTests
{
    [Fact]
    public void Constructor_PaddedName_SetsTrimmedName()
    {
        // Arrange
        var id = Guid.Parse("11111111-1111-4111-8111-111111110001");
        const string name = "  Category 1  ";

        // Act
        var category = new Category(id, name);

        // Assert
        Assert.Equal("Category 1", category.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_BlankName_ThrowsDomainValidationException(string? name)
    {
        // Arrange
        var id = Guid.Parse("11111111-1111-4111-8111-111111110001");

        // Act
        var act = () => new Category(id, name!);

        // Assert
        Assert.Throws<DomainValidationException>(act);
    }
}
