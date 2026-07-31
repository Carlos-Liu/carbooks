namespace CarBooks.Domain.Shared.Errors;

/// <summary>
/// Base type for every error the application deliberately raises and knows how to translate
/// into an HTTP response. Anything not derived from this is treated as an unexpected failure.
/// </summary>
public abstract class CarBooksException : Exception
{
    protected CarBooksException(int errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public int ErrorCode { get; }
}
