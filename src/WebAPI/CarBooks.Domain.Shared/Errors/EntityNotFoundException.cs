namespace CarBooks.Domain.Shared.Errors;

/// <summary>
/// Raised when a requested aggregate does not exist.
/// </summary>
public sealed class EntityNotFoundException : CarBooksException
{
    public EntityNotFoundException(string entityName, object key)
        : base(CatalogErrorCodes.EntityNotFound, $"{entityName} '{key}' was not found.")
    {
        EntityName = entityName;
        Key = key;
    }

    public string EntityName { get; }

    public object Key { get; }
}
