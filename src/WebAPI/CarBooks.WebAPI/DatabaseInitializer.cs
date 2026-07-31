using CarBooks.Database.Ef;
using CarBooks.Database.Ef.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CarBooks.WebAPI;

/// <summary>
/// Applies pending EF Core migrations and writes the starter catalog before the API starts serving
/// traffic. Convenient for local development and container demos; a production pipeline would run
/// migrations as a separate deployment step instead.
/// </summary>
internal sealed class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<DatabaseInitializer> logger;

    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarBooksDbContext>();

        logger.LogInformation("Applying database migrations.");
        await dbContext.Database.MigrateAsync(cancellationToken);

        var seeder = scope.ServiceProvider.GetRequiredService<CarBooksDbSeeder>();
        var seeded = await seeder.SeedAsync(cancellationToken);

        logger.LogInformation(
            seeded ? "Database seeded with the starter catalog." : "Database already contains catalog data.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
