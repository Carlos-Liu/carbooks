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

    public static BookDto ToDto(
        this Book book,
        Func<Guid, string> thumbnailUrlFactory,
        IReadOnlyList<Tag>? tags = null) =>
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
            book.HasCoverThumbnail ? thumbnailUrlFactory(book.Id) : null,
            (tags ?? []).Select(tag => tag.ToDto()).ToList());

    public static IReadOnlyList<BookDto> ToDtos(
        this IEnumerable<Book> books,
        Func<Guid, string> thumbnailUrlFactory,
        IReadOnlyDictionary<Guid, IReadOnlyList<Tag>> tagsByBookId)
    {
        ArgumentNullException.ThrowIfNull(books);
        return books
                .Select(book => book.ToDto(
                    thumbnailUrlFactory,
                    tagsByBookId.GetValueOrDefault(book.Id)))
                .ToList();
    }
}
