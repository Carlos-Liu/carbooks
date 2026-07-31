namespace CarBooks.Domain.Shared.Errors;

/// <summary>
/// Raised when an operation would leave an aggregate in a state that violates a business invariant.
/// </summary>
public sealed class DomainValidationException : CarBooksException
{
    public DomainValidationException(string message)
        : base(CatalogErrorCodes.DomainValidationFailed, message)
    {
    }
}
