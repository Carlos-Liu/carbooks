namespace CarBooks.Domain.Repositories;

/// <summary>
/// Coordinates a single persistence boundary for the current scope (typically one HTTP request).
/// Repositories only track changes; callers flush once via <see cref="SaveChangesAsync"/>.
/// </summary>
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
