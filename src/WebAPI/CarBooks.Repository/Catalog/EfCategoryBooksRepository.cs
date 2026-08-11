using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;

namespace CarBooks.Repository.Catalog;

internal sealed class EfCategoryBooksRepository : ICategoryBooksRepository
{
    private readonly CarBooksDbContext dbContext;

    public EfCategoryBooksRepository(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddRangeAsync(IEnumerable<CategoryBooks> links, CancellationToken cancellationToken)
    {
        await dbContext.CategoryBooks.AddRangeAsync(links, cancellationToken);
    }
}
