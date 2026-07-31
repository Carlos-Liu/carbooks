using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain;

/// <summary>
/// Invariant checks used by entity constructors and mutators. Failures surface as
/// <see cref="DomainValidationException"/> so the API can translate them into 400 responses.
/// </summary>
public static class Guard
{
    public static string Text(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException($"{fieldName} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new DomainValidationException($"{fieldName} must be {maxLength} characters or fewer.");
        }

        return trimmed;
    }

    public static string AbsoluteUrl(string? value, string fieldName, int maxLength)
    {
        var trimmed = Text(value, fieldName, maxLength);
        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainValidationException($"{fieldName} must be an absolute http or https URL.");
        }

        return trimmed;
    }

    public static int NotNegative(int value, string fieldName)
    {
        if (value < 0)
        {
            throw new DomainValidationException($"{fieldName} must not be negative.");
        }

        return value;
    }
}
