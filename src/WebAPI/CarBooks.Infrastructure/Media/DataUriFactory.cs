namespace CarBooks.Infrastructure.Media;

internal sealed class DataUriFactory : IDataUriFactory
{
    public string? Create(byte[]? content, string? contentType)
    {
        if (content is null || content.Length == 0 || string.IsNullOrWhiteSpace(contentType))
        {
            return null;
        }

        return $"data:{contentType};base64,{Convert.ToBase64String(content)}";
    }
}
