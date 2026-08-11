using CarBooks.Database.Ef;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyDictionary<Guid, IReadOnlyList<Tag>>> ListTagsByBookIdsAsync(
        IEnumerable<Guid> bookIds,
        CancellationToken cancellationToken)
    {
        var distinctBookIds = bookIds.Distinct().ToList();
        if (distinctBookIds.Count == 0)
        {
            return new Dictionary<Guid, IReadOnlyList<Tag>>();
        }

        var rows = await (
                from link in dbContext.BookTags.AsNoTracking()
                join tag in dbContext.Tags.AsNoTracking() on link.TagId equals tag.Id
                where distinctBookIds.Contains(link.BookId)
                orderby tag.Name
                select new { link.BookId, Tag = tag })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(row => row.BookId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<Tag>)group.Select(row => row.Tag).ToList());
    }
}
