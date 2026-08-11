using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarBooks.Repository.Catalog;

internal sealed class EfTagRepository : ITagRepository
{
    private readonly CarBooksDbContext dbContext;

    public EfTagRepository(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Tag>> ListAsync(CancellationToken cancellationToken) =>
        await dbContext.Tags
            .AsNoTracking()
            .OrderBy(tag => tag.Name)
            .ToListAsync(cancellationToken);

    public Task<Tag?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);
}
