using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// A book belonging to exactly one <see cref="Category"/>.
/// </summary>
/// <remarks>
/// A cover can be supplied two ways: <see cref="CoverUrl"/> always points at the publisher artwork,
/// while <see cref="CoverImage"/> optionally holds a locally stored copy that keeps the catalog
/// renderable when the external host is unreachable.
/// </remarks>
public sealed class Book : Entity
{
    internal Book(Guid id, Guid categoryId, string name, string author, string coverUrl, int displayOrder)
        : base(id)
    {
        if (categoryId == Guid.Empty)
        {
            throw new DomainValidationException("A book must belong to a category.");
        }

        CategoryId = categoryId;
        Name = Guard.Text(name, nameof(Name), CatalogConsts.MaxBookNameLength);
        Author = Guard.Text(author, nameof(Author), CatalogConsts.MaxBookAuthorLength);
        CoverUrl = Guard.AbsoluteUrl(coverUrl, nameof(CoverUrl), CatalogConsts.MaxCoverUrlLength);
        DisplayOrder = Guard.NotNegative(displayOrder, nameof(DisplayOrder));
    }

    private Book()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public string Author { get; private set; } = string.Empty;

    /// <summary>Absolute URL of the cover artwork hosted outside the application.</summary>
    public string CoverUrl { get; private set; } = string.Empty;

    /// <summary>Locally stored cover artwork, or <see langword="null"/> when only the URL is known.</summary>
    public byte[]? CoverImage { get; private set; }

    public string? CoverImageContentType { get; private set; }

    public Guid CategoryId { get; private set; }

    public Category? Category { get; private set; }

    public int DisplayOrder { get; private set; }

    public bool HasCoverImage =>
        CoverImage is { Length: > 0 } && !string.IsNullOrWhiteSpace(CoverImageContentType);

    public void SetCoverImage(byte[] content, string contentType)
    {
        if (content.Length == 0)
        {
            throw new DomainValidationException("Cover image content must not be empty.");
        }

        CoverImage = content;
        CoverImageContentType = Guard.Text(contentType, nameof(CoverImageContentType), CatalogConsts.MaxContentTypeLength);
    }

    public void ClearCoverImage()
    {
        CoverImage = null;
        CoverImageContentType = null;
    }
}
