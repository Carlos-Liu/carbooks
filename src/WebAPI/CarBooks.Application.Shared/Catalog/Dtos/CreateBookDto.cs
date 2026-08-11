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

    /// <summary>Translator of the book.</summary>
    [MaxLength(Consts.MaxBookAuthorLength)]
    public string? Translator { get; init; }

    /// <summary>Publisher name.</summary>
    [MaxLength(Consts.MaxBookPublisherLength)]
    public string? Publisher { get; init; }

    /// <summary>Publication date without a time component.</summary>
    public DateOnly? PublishedOn { get; init; }

    /// <summary>Short recommendation blurb.</summary>
    [MaxLength(Consts.MaxBookRecommendationLength)]
    public string? Recommendation { get; init; }

    /// <summary>ISBN of the book.</summary>
    [MaxLength(Consts.MaxBookIsbnLength)]
    public string? Isbn { get; init; }

    /// <summary>Absolute URL of the publisher cover artwork.</summary>
    [MaxLength(Consts.MaxCoverUrlLength)]
    public string? CoverUrl { get; init; }

    /// <summary>Optional cover image selected from the caller's local machine.</summary>
    public IFormFile? CoverImage { get; init; }

    /// <summary>Optional category identifiers to assign (zero or more).</summary>
    public IList<Guid> CategoryIds { get; init; } = new List<Guid>();

    /// <summary>Optional tag identifiers to assign (zero or more).</summary>
    public IList<Guid> TagIds { get; init; } = new List<Guid>();
}
