namespace CarBooks.Domain.Shared.Errors;

/// <summary>
/// Numeric error identifiers returned to API clients. Each area owns a reserved segment so codes
/// stay stable as the application grows; pick the next unused value inside a segment.
/// </summary>
public static class CatalogErrorCodes
{
    /// <summary>Common segment: 1001-1099.</summary>
    public const int DomainValidationFailed = 1001;

    /// <summary>Common segment: 1001-1099.</summary>
    public const int EntityNotFound = 1002;

    /// <summary>Common segment: 1001-1099.</summary>
    public const int UnexpectedFailure = 1003;
}
