using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Media;
using Microsoft.EntityFrameworkCore;

namespace CarBooks.Repository.Catalog;

internal sealed class EfBookRepository : IBookRepository
{
    private readonly CarBooksDbContext dbContext;

    public EfBookRepository(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Book>> ListByCategoryAsync(Guid categoryId, CancellationToken cancellationToken) =>
        await dbContext.Books
            .AsNoTracking()
            .Where(book => dbContext.CategoryBooks.Any(link =>
                link.BookId == book.Id && link.CategoryId == categoryId))
            .OrderBy(book => book.Name)
            .ToListAsync(cancellationToken);

    public Task<Book?> FindAsync(Guid bookId, CancellationToken cancellationToken) =>
        dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == bookId, cancellationToken);

    public async Task<ImageContent?> FindCoverThumbnailAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var thumbnail = await dbContext.Books
            .AsNoTracking()
            .Where(book => book.Id == bookId)
            .Select(book => new
            {
                book.CoverThumbnail,
                book.CoverThumbnailContentType,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (thumbnail?.CoverThumbnail is not { Length: > 0 } || string.IsNullOrWhiteSpace(thumbnail.CoverThumbnailContentType))
        {
            return null;
        }

        return new ImageContent(thumbnail.CoverThumbnail, thumbnail.CoverThumbnailContentType);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken)
    {
        await dbContext.Books.AddAsync(book, cancellationToken);
    }
}
