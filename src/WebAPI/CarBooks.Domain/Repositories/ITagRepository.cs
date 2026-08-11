using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface ITagRepository
{
    Task<IReadOnlyList<Tag>> ListAsync(CancellationToken cancellationToken);

    Task<Tag?> FindAsync(Guid id, CancellationToken cancellationToken);
}
