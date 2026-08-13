using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface ITagRepository
{
    Task<IReadOnlyList<Tag>> ListAsync(CancellationToken cancellationToken);

    Task<Tag?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Tag>> FindByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}
