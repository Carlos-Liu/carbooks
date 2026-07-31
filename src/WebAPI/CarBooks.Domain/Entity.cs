namespace CarBooks.Domain;

/// <summary>
/// Base class for aggregate roots and entities. Identity is assigned by the domain rather than by
/// the database so aggregates are fully valid before they are ever persisted.
/// </summary>
public abstract class Entity
{
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Identity must not be an empty GUID.", nameof(id));
        }

        Id = id;
    }

    /// <summary>Constructor used by EF Core when materialising entities.</summary>
    protected Entity()
    {
    }

    public Guid Id { get; private set; }

    public static Guid NewId() => Guid.CreateVersion7();
}
