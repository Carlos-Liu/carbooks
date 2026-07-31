using CarBooks.Domain.Catalog;

namespace CarBooks.Domain.Repositories;

public interface IBookRepository
{
    /// <summary>Returns the books of a category ordered for presentation.</summary>
    Task<IReadOnlyList<Book>> ListByCategoryAsync(Guid categoryId, CancellationToken cancellationToken);

    Task<Book?> FindAsync(Guid bookId, CancellationToken cancellationToken);
}
