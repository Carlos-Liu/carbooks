using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// A reusable label that can be attached to many books via <see cref="BookTags"/>.
/// </summary>
public sealed class Tag : Entity
{
    public Tag(Guid id, string name)
        : base(id)
    {
        Name = Guard.Text(name, nameof(Name));
    }

    private Tag()
    {
    }

    [Required]
    [MaxLength(Consts.MaxTagNameLength)]
    public string Name { get; private set; } = string.Empty;
}
