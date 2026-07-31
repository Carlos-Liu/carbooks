namespace CarBooks.Domain.Shared;

/// <summary>
/// Field limits shared by the domain model, the EF Core mapping and the API contract so all three
/// agree on what a valid catalog record looks like.
/// </summary>
public static class CatalogConsts
{
    public const int MaxCategoryNameLength = 64;

    public const int MaxBookNameLength = 64;

    public const int MaxBookAuthorLength = 64;

    public const int MaxCoverUrlLength = 2048;

    public const int MaxContentTypeLength = 128;
}
