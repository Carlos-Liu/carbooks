using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface IBookTagsRepository
{
    Task AddRangeAsync(IEnumerable<BookTags> links, CancellationToken cancellationToken);

    /// <summary>
    /// Returns tags keyed by book id for the given books. Books with no tags are omitted.
    /// </summary>
    Task<IReadOnlyDictionary<Guid, IReadOnlyList<Tag>>> ListTagsByBookIdsAsync(
        IEnumerable<Guid> bookIds,
        CancellationToken cancellationToken);
}
