namespace CarBooks.Infrastructure.Media;

/// <summary>
/// Converts binary media stored alongside an entity into an inline <c>data:</c> URI that a browser
/// can render directly, which avoids a second round trip per image.
/// </summary>
public interface IDataUriFactory
{
    /// <summary>
    /// Builds a <c>data:</c> URI, or returns <see langword="null"/> when there is nothing to encode.
    /// </summary>
    string? Create(byte[]? content, string? contentType);
}
