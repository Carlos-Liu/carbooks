using CarBooks.Domain.Shared.Errors;
using CarBooks.Domain.Shared.Media;
using SkiaSharp;

namespace CarBooks.Infrastructure.Media;

internal sealed class CoverThumbnailGenerator : ICoverThumbnailGenerator
{
    private const int ThumbnailWidth = 160;
    private const int ThumbnailQuality = 85;

    /// <summary>
    /// Generates a thumbnail image from the provided content and content type.
    /// The thumbnail will have a width of 160 pixels and a quality of 85. The height
    /// will be calculated to maintain the original aspect ratio of the image.
    /// The output content type will be normalized based on the input content type.
    /// </summary>
    /// <param name="content">The image content to generate a thumbnail from.</param>
    /// <param name="contentType">The content type of the image.</param>
    /// <returns>An <see cref="ImageContent"/> representing the generated thumbnail.</returns>
    /// <exception cref="DomainValidationException">Thrown when the image content is invalid or the thumbnail cannot be generated.</exception>

    public ImageContent Generate(byte[] content, string contentType)
    {
        if (content.Length == 0)
        {
            throw new DomainValidationException("Cover image content is required to generate a thumbnail.");
        }

        using var bitmap = SKBitmap.Decode(content);
        if (bitmap is null || bitmap.Width <= 0 || bitmap.Height <= 0)
        {
            throw new DomainValidationException("Cover image must be a valid image file.");
        }

        var thumbnailHeight = Math.Max(1, (int)Math.Round(bitmap.Height * (ThumbnailWidth / (double)bitmap.Width)));
        using var resized = bitmap.Resize(
            new SKImageInfo(ThumbnailWidth, thumbnailHeight),
            new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        if (resized is null)
        {
            throw new DomainValidationException("Cover thumbnail could not be generated.");
        }

        using var image = SKImage.FromBitmap(resized);
        using var encoded = image.Encode(GetEncodedImageFormat(contentType), ThumbnailQuality);

        return new ImageContent(encoded.ToArray(), NormalizeOutputContentType(contentType));
    }

    private static SKEncodedImageFormat GetEncodedImageFormat(string contentType) =>
        contentType.ToLowerInvariant() switch
        {
            "image/png" => SKEncodedImageFormat.Png,
            "image/gif" => SKEncodedImageFormat.Png,
            "image/webp" => SKEncodedImageFormat.Webp,
            _ => SKEncodedImageFormat.Jpeg,
        };

    private static string NormalizeOutputContentType(string contentType) =>
        contentType.ToLowerInvariant() switch
        {
            "image/png" => "image/png",
            "image/gif" => "image/png",
            "image/webp" => "image/webp",
            _ => "image/jpeg",
        };
}
