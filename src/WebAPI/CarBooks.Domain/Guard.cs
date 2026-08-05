using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain;

/// <summary>
/// Invariant checks used by entity constructors and mutators. Failures surface as
/// <see cref="DomainValidationException"/> so the API can translate them into 400 responses.
/// Length limits live on the entities as <c>[MaxLength]</c> and are applied by EF / validators.
/// </summary>
public static class Guard
{
    public static string Text(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException($"{fieldName} is required.");
        }

        return value.Trim();
    }

    /// <summary>Trims optional text; blank input becomes <see langword="null"/>.</summary>
    public static string? OptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    public static string AbsoluteUrl(string? value, string fieldName)
    {
        var trimmed = Text(value, fieldName);
        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainValidationException($"{fieldName} must be an absolute http or https URL.");
        }

        return trimmed;
    }

    /// <summary>
    /// Optional absolute http(s) URL; blank input becomes <see langword="null"/>.
    /// </summary>
    public static string? OptionalAbsoluteUrl(string? value, string fieldName)
    {
        var trimmed = OptionalText(value);
        if (trimmed is null)
        {
            return null;
        }

        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainValidationException($"{fieldName} must be an absolute http or https URL.");
        }

        return trimmed;
    }
}
