using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace CarBooks.Application.Shared.Catalog.Dtos;

/// <summary>
/// Payload for creating a book. Sent as <c>multipart/form-data</c> so the caller can upload a
/// local cover image file together with the text fields.
/// </summary>
public sealed class CreateBookDto
{
    /// <summary>Title of the book.</summary>
    [Required]
    [MaxLength(Consts.MaxBookNameLength)]
    public string Name { get; init; } = string.Empty;

    /// <summary>Author of the book.</summary>
    [Required]
    [MaxLength(Consts.MaxBookAuthorLength)]
    public string Author { get; init; } = string.Empty;

    /// <summary>Absolute URL of the publisher cover artwork.</summary>
    [Required]
    [MaxLength(Consts.MaxCoverUrlLength)]
    public string CoverUrl { get; init; } = string.Empty;

    /// <summary>Optional cover image selected from the caller's local machine.</summary>
    public IFormFile? CoverImage { get; init; }
}
