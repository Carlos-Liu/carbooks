using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarBooks.Repository.Catalog;

internal sealed class EfCategoryRepository : ICategoryRepository
{
    private readonly CarBooksDbContext dbContext;

    public EfCategoryRepository(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken) =>
        await dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);

    public Task<Category?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

    public async Task<List<Category>> FindByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        await dbContext.Categories
            .AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyDictionary<Guid, int>> CountBooksByCategoryAsync(CancellationToken cancellationToken) =>
        await dbContext.CategoryBooks
            .AsNoTracking()
            .GroupBy(link => link.CategoryId)
            .Select(group => new { CategoryId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(row => row.CategoryId, row => row.Count, cancellationToken);
}
