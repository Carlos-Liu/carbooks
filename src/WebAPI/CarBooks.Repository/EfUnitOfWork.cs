using CarBooks.Database.Ef;
using CarBooks.Domain.Repositories;

namespace CarBooks.Repository;

internal sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly CarBooksDbContext dbContext;

    public EfUnitOfWork(CarBooksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
