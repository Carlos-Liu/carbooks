namespace CarBooks.Domain.Shared;

/// <summary>
/// Field limits used by entity <c>[MaxLength]</c> attributes (and thus by EF Core conventions) and
/// by API contracts so they agree on what a valid catalog record looks like.
/// </summary>
public static class CatalogConsts
{
    public const int MaxCategoryNameLength = 64;

    public const int MaxBookNameLength = 64;

    public const int MaxBookAuthorLength = 64;

    public const int MaxCoverUrlLength = 2048;

    public const int MaxContentTypeLength = 128;
}
