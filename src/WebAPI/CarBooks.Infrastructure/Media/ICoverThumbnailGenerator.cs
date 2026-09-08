using CarBooks.Domain.Shared.Media;

namespace CarBooks.Infrastructure.Media;

public interface ICoverThumbnailGenerator
{
    /// <summary>
    /// Generates a thumbnail image from the provided content and content type.
    /// The output content type will be normalized based on the input content type.
    /// </summary>
    /// <param name="content">The image content to generate a thumbnail from.</param>
    /// <param name="contentType">The content type of the image.</param>
    /// <returns>An <see cref="ImageContent"/> representing the generated thumbnail.</returns>
    /// <exception cref="DomainValidationException">Thrown when the image content is invalid or the thumbnail cannot be generated.</exception>

    ImageContent Generate(byte[] content, string contentType);
}
