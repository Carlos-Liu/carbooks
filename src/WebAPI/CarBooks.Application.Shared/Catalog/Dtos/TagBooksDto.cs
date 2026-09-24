namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// Payload of the tag page: the tag itself plus the books labeled by it, so the SPA can
/// render the heading and the list from a single request.
/// </summary>
public sealed record TagBooksDto(TagDto Tag, IReadOnlyList<BookDto> Books);
