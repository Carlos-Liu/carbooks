using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface ICategoryBooksRepository
{
    Task AddRangeAsync(IEnumerable<CategoryBooks> links, CancellationToken cancellationToken);
}
