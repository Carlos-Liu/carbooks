namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// A book as presented on a category page.
/// </summary>
/// <param name="Id">Stable identifier of the book.</param>
/// <param name="Name">Title of the book.</param>
/// <param name="Author">Author of the book.</param>
/// <param name="Translator">Translator of the book, if any.</param>
/// <param name="Publisher">Publisher name, if any.</param>
/// <param name="PublishedOn">Publication date (date only), if known.</param>
/// <param name="Recommendation">Short recommendation blurb, if any.</param>
/// <param name="Isbn">ISBN, if known.</param>
/// <param name="CoverUrl">Absolute URL of the publisher cover artwork, if any.</param>
/// <param name="CoverImage">
/// Locally stored cover artwork encoded as a <c>data:</c> URI, or <see langword="null"/> when no
/// local image is stored.
/// </param>
public sealed record BookDto(
    Guid Id,
    string Name,
    string Author,
    string? Translator,
    string? Publisher,
    DateOnly? PublishedOn,
    string? Recommendation,
    string? Isbn,
    string? CoverUrl,
    string? CoverImage);
