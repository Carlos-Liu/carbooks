using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// An independent book that may belong to zero or more <see cref="Category"/> entries.
/// </summary>
/// <remarks>
/// A cover can be supplied two ways: <see cref="CoverUrl"/> may point at publisher artwork,
/// while <see cref="CoverImage"/> optionally holds a locally stored copy that keeps the catalog
/// renderable when the external host is unreachable.
/// </remarks>
public sealed class Book : Entity
{
    private readonly List<Category> categories = [];

    public Book(
        Guid id,
        string name,
        string author,
        string? coverUrl = null,
        string? translator = null,
        string? publisher = null,
        DateOnly? publishedOn = null,
        string? recommendation = null,
        string? isbn = null)
        : base(id)
    {
        Name = Guard.Text(name, nameof(Name));
        Author = Guard.Text(author, nameof(Author));
        CoverUrl = Guard.OptionalAbsoluteUrl(coverUrl, nameof(CoverUrl));
        Translator = Guard.OptionalText(translator);
        Publisher = Guard.OptionalText(publisher);
        PublishedOn = publishedOn;
        Recommendation = Guard.OptionalText(recommendation);
        Isbn = Guard.OptionalText(isbn);
    }

    private Book()
    {
    }

    [Required]
    [MaxLength(Consts.MaxBookNameLength)]
    public string Name { get; private set; } = string.Empty;

    [Required]
    [MaxLength(Consts.MaxBookAuthorLength)]
    public string Author { get; private set; } = string.Empty;

    [MaxLength(Consts.MaxBookAuthorLength)]
    public string? Translator { get; private set; }

    [MaxLength(Consts.MaxBookPublisherLength)]
    public string? Publisher { get; private set; }

    /// <summary>Publication date without a time component.</summary>
    public DateOnly? PublishedOn { get; private set; }

    [MaxLength(Consts.MaxBookRecommendationLength)]
    public string? Recommendation { get; private set; }

    [MaxLength(Consts.MaxBookIsbnLength)]
    public string? Isbn { get; private set; }

    /// <summary>Absolute URL of the cover artwork hosted outside the application.</summary>
    [MaxLength(Consts.MaxCoverUrlLength)]
    public string? CoverUrl { get; private set; }

    /// <summary>Locally stored cover artwork, or <see langword="null"/> when only the URL is known.</summary>
    public byte[]? CoverImage { get; private set; }

    [MaxLength(Consts.MaxContentTypeLength)]
    public string? CoverImageContentType { get; private set; }

    public IReadOnlyList<Category> Categories => categories;

    public bool HasCoverImage =>
        CoverImage is { Length: > 0 } && !string.IsNullOrWhiteSpace(CoverImageContentType);

    public void AssignToCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (categories.Any(existing => existing.Id == category.Id))
        {
            return;
        }

        categories.Add(category);
    }

    public void RemoveFromCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        categories.RemoveAll(existing => existing.Id == category.Id);
    }

    public void SetCoverImage(byte[] content, string contentType)
    {
        if (content.Length == 0)
        {
            throw new DomainValidationException("Cover image content must not be empty.");
        }

        CoverImage = content;
        CoverImageContentType = Guard.Text(contentType, nameof(CoverImageContentType));
    }

    public void ClearCoverImage()
    {
        CoverImage = null;
        CoverImageContentType = null;
    }
}
