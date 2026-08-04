using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Aggregate root representing a catalog grouping used for browsing.
/// </summary>
public sealed class Category : Entity
{
    public Category(Guid id, string name)
        : base(id)
    {
        Name = Guard.Text(name, nameof(Name));
    }

    private Category()
    {
    }

    [Required]
    [MaxLength(Consts.MaxCategoryNameLength)]
    public string Name { get; private set; } = string.Empty;
}
