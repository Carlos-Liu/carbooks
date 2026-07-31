namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// Payload of the category page: the category itself plus the books it contains, so the SPA can
/// render the heading and the list from a single request.
/// </summary>
public sealed record CategoryBooksDto(CategoryDto Category, IReadOnlyList<BookDto> Books);
