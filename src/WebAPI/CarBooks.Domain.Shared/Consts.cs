namespace CarBooks.Domain.Shared;

/// <summary>
/// Field limits used by entity <c>[MaxLength]</c> attributes (and thus by EF Core conventions) and
/// by API contracts so they agree on what a valid catalog record looks like.
/// </summary>
public static class Consts
{
    public const int MaxCategoryNameLength = 64;

    public const int MaxBookNameLength = 64;

    public const int MaxBookAuthorLength = 64;

    public const int MaxCoverUrlLength = 2048;

    public const int MaxContentTypeLength = 128;

    /// <summary>Maximum size of an uploaded cover image in bytes (5 MB).</summary>
    public const int MaxCoverImageBytes = 5 * 1024 * 1024;
}
