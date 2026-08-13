using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface ICategoryRepository
{
    /// <summary>Returns every category ordered for presentation.</summary>
    Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken);

    Task<Category?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Category>> FindByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<Guid, int>> CountBooksByCategoryAsync(CancellationToken cancellationToken);
}
