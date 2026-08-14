namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// Host-agnostic cover image payload passed into the application layer.
/// </summary>
public sealed class CoverImageDto
{
    public byte[] Content { get; init; } = [];

    public string? ContentType { get; init; }
}
