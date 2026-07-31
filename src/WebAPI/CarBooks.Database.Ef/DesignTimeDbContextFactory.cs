using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarBooks.Database.Ef;

/// <summary>
/// Used only by <c>dotnet ef</c> when it needs a context outside the running application. The
/// connection string is never opened for <c>migrations add</c>; it only has to be well formed.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarBooksDbContext>
{
    private const string DesignTimeConnectionString =
        "Host=localhost;Port=15433;Database=carbooks;Username=carbooks;Password=carbooks";

    public CarBooksDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("CARBOOKS_DESIGN_TIME_CONNECTION")
            ?? DesignTimeConnectionString;

        var options = new DbContextOptionsBuilder<CarBooksDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new CarBooksDbContext(options);
    }
}
