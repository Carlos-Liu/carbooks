namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// A reusable book tag for filtering and labeling.
/// </summary>
/// <param name="Id">Stable identifier of the tag.</param>
/// <param name="Name">Display name, for example <c>Racing</c>.</param>
public sealed record TagDto(Guid Id, string Name);
