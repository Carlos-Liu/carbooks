using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Catalog;
using CarBooks.Infrastructure.Media;

namespace CarBooks.Application.Catalog.Mapping;

/// <summary>
/// Hand-written translation between domain entities and API contracts. Mapping is explicit on
/// purpose: it keeps the projection reviewable and prevents domain state from leaking into the API
/// by accident.
/// </summary>
internal static class Mappings
{
    public static CategoryDto ToDto(this Category category, int bookCount) =>
        new(category.Id, category.Name, bookCount);

    public static TagDto ToDto(this Tag tag) =>
        new(tag.Id, tag.Name);

    public static BookDto ToDto(this Book book, IDataUriFactory dataUriFactory) =>
        new(
            book.Id,
            book.Name,
            book.Author,
            book.Translator,
            book.Publisher,
            book.PublishedOn,
            book.Recommendation,
            book.Isbn,
            book.CoverUrl,
            dataUriFactory.Create(book.CoverImage, book.CoverImageContentType));

    public static IReadOnlyList<BookDto> ToDtos(this IEnumerable<Book> books, IDataUriFactory dataUriFactory) =>
        books.Select(book => book.ToDto(dataUriFactory)).ToList();
}
