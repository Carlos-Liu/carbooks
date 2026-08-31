namespace CarBooks.Domain.Shared;

/// <summary>
/// Field limits used by entity <c>[MaxLength]</c> attributes (and thus by EF Core conventions) and
/// by API contracts so they agree on what a valid catalog record looks like.
/// </summary>
public static class Consts
{
    public const int MaxCategoryNameLength = 64;

    public const int MaxTagNameLength = 32;

    public const int MaxBookNameLength = 64;

    public const int MaxBookAuthorLength = 64;

    public const int MaxBookPublisherLength = 32;

    public const int MaxBookRecommendationLength = 1024;

    public const int MaxBookIsbnLength = 32;

    public const int MaxCoverUrlLength = 2048;

    public const int MaxContentTypeLength = 128;

    /// <summary>Maximum size of an uploaded cover image in bytes (2 MB).</summary>
    public const int MaxCoverImageBytes = 2 * 1024 * 1024;
}
