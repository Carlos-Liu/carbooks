using Microsoft.EntityFrameworkCore;

namespace CarBooks.Database.Ef.Seeding;

/// <summary>
/// Populates an empty database with the starter catalog. Seeding is skipped as soon as any category
/// exists, so it is safe to run on every start-up.
/// </summary>
public sealed class CarBooksDbSeeder
{
    private readonly CarBooksDbContext dbContext;

    public CarBooksDbSeeder(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <returns><see langword="true"/> when seed data was written.</returns>
    public async Task<bool> SeedAsync(CancellationToken cancellationToken)
    {
        if (await dbContext.Categories.AnyAsync(cancellationToken))
        {
            return false;
        }

        await dbContext.Categories.AddRangeAsync(CatalogSeedData.CreateCategories(), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
