namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// A catalog category as presented on the main page.
/// </summary>
/// <param name="Id">Stable identifier of the category.</param>
/// <param name="Name">Display name, for example <c>Category 1</c>.</param>
/// <param name="Slug">URL-safe identifier used by the SPA route.</param>
/// <param name="BookCount">Number of books the category contains.</param>
public sealed record CategoryDto(Guid Id, string Name, string Slug, int BookCount);
