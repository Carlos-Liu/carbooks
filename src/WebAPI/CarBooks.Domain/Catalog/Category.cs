using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Aggregate root representing a catalog grouping used for browsing.
/// </summary>
public sealed class Category : Entity
{
    public Category(Guid id, string name, int displayOrder)
        : base(id)
    {
        DisplayOrder = Guard.NotNegative(displayOrder, nameof(DisplayOrder));
    }

    private Category()
    {
    }

    [Required]
    [MaxLength(CatalogConsts.MaxCategoryNameLength)]
    public string Name { get; private set; } = string.Empty;

    public int DisplayOrder { get; private set; }

    public void Rename(string name) =>
        Name = Guard.Text(name, nameof(Name), CatalogConsts.MaxCategoryNameLength);
}
