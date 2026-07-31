using CarBooks.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace CarBooks.Database.Ef;

public sealed class CarBooksDbContext : DbContext
{
    public CarBooksDbContext(DbContextOptions<CarBooksDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarBooksDbContext).Assembly);
    }
}
