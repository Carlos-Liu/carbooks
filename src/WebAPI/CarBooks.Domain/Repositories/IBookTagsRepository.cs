using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface IBookTagsRepository
{
    Task AddRangeAsync(IEnumerable<BookTags> links, CancellationToken cancellationToken);
}
