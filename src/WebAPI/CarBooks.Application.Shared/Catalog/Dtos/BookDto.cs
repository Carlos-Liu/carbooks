namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// A book as presented on a category page.
/// </summary>
/// <param name="Id">Stable identifier of the book.</param>
/// <param name="Name">Title of the book.</param>
/// <param name="Author">Author of the book.</param>
/// <param name="CoverUrl">Absolute URL of the publisher cover artwork.</param>
/// <param name="CoverImage">
/// Locally stored cover artwork encoded as a <c>data:</c> URI, or <see langword="null"/> when the
/// book only has an external <paramref name="CoverUrl"/>.
/// </param>
public sealed record BookDto(
    Guid Id,
    string Name,
    string Author,
    string CoverUrl,
    string? CoverImage);
