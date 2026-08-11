using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;

namespace CarBooks.Repository.Catalog;

internal sealed class EfBookTagsRepository : IBookTagsRepository
{
    private readonly CarBooksDbContext dbContext;

    public EfBookTagsRepository(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddRangeAsync(IEnumerable<BookTags> links, CancellationToken cancellationToken)
    {
        await dbContext.BookTags.AddRangeAsync(links, cancellationToken);
    }
}
