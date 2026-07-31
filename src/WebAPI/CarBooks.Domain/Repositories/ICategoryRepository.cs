using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface ICategoryRepository
{
    /// <summary>Returns every category ordered for presentation.</summary>
    Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken);

    Task<Category?> FindBySlugAsync(string slug, CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<Guid, int>> CountBooksByCategoryAsync(CancellationToken cancellationToken);
}
