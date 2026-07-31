using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
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
            .Where(book => book.Categories.Any(category => category.Id == categoryId))
            .OrderBy(book => book.DisplayOrder)
            .ThenBy(book => book.Name)
            .ToListAsync(cancellationToken);

    public Task<Book?> FindAsync(Guid bookId, CancellationToken cancellationToken) =>
        dbContext.Books
            .AsNoTracking()
            .Include(book => book.Categories)
            .FirstOrDefaultAsync(book => book.Id == bookId, cancellationToken);
}
