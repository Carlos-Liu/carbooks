using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared.Media;

namespace CarBooks.Domain.Repositories;

public interface IBookRepository
{
    /// <summary>Returns the books of a category ordered for presentation.</summary>
    Task<IReadOnlyList<Book>> ListByCategoryAsync(Guid categoryId, CancellationToken cancellationToken);

    /// <summary>Returns the books labeled with a tag, ordered for presentation.</summary>
    Task<IReadOnlyList<Book>> ListByTagAsync(Guid tagId, CancellationToken cancellationToken);

    Task<Book?> FindAsync(Guid bookId, CancellationToken cancellationToken);

    Task<ImageContent?> FindCoverThumbnailAsync(Guid bookId, CancellationToken cancellationToken);

    Task AddAsync(Book book, CancellationToken cancellationToken);
}
