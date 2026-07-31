using CarBooks.Domain.Catalog;

namespace CarBooks.Database.Ef.Seeding;

/// <summary>
/// The starter catalog: three categories with three books each. Identifiers are fixed so repeated
/// seeding across environments produces the same rows.
/// </summary>
internal static class CatalogSeedData
{
    public static IReadOnlyList<Category> CreateCategories() =>
    [
        CreateCategory(
            id: "11111111-1111-4111-8111-111111110001",
            name: "Category 1",
            slug: "category-1",
            displayOrder: 1,
            topColor: "#334155",
            bottomColor: "#0f172a",
            books:
            [
                new SeedBook("22222222-2222-4222-8222-222222220001", "Go Like Hell", "A. J. Baime"),
                new SeedBook("22222222-2222-4222-8222-222222220002", "The Limit", "Michael Cannell"),
                new SeedBook("22222222-2222-4222-8222-222222220003", "Ferrari Rex", "Luca Dal Monte"),
            ]),
        CreateCategory(
            id: "11111111-1111-4111-8111-111111110002",
            name: "Category 2",
            slug: "category-2",
            displayOrder: 2,
            topColor: "#9f1239",
            bottomColor: "#4c0519",
            books:
            [
                new SeedBook("22222222-2222-4222-8222-222222220004", "How to Build a Car", "Adrian Newey"),
                new SeedBook("22222222-2222-4222-8222-222222220005", "Total Competition", "Ross Brawn and Adam Parr"),
                new SeedBook("22222222-2222-4222-8222-222222220006", "The Mechanic", "Marc Priestley"),
            ]),
        CreateCategory(
            id: "11111111-1111-4111-8111-111111110003",
            name: "Category 3",
            slug: "category-3",
            displayOrder: 3,
            topColor: "#166534",
            bottomColor: "#052e16",
            books:
            [
                new SeedBook("22222222-2222-4222-8222-222222220007", "Faster", "Neal Bascomb"),
                new SeedBook("22222222-2222-4222-8222-222222220008", "The Art of the Formula 1 Race Car", "Stuart Codling"),
                new SeedBook("22222222-2222-4222-8222-222222220009", "Car Guys vs. Bean Counters", "Bob Lutz"),
            ]),
    ];

    private static Category CreateCategory(
        string id,
        string name,
        string slug,
        int displayOrder,
        string topColor,
        string bottomColor,
        IReadOnlyList<SeedBook> books)
    {
        var category = new Category(Guid.Parse(id), name, slug, displayOrder);

        for (var index = 0; index < books.Count; index++)
        {
            var seed = books[index];
            var book = category.AddBook(
                Guid.Parse(seed.Id),
                seed.Name,
                seed.Author,
                BuildCoverUrl(seed.Name, topColor),
                index + 1);

            book.SetCoverImage(
                PlaceholderCover.Create(seed.Name, seed.Author, topColor, bottomColor),
                PlaceholderCover.ContentType);
        }

        return category;
    }

    private static string BuildCoverUrl(string title, string background) =>
        $"https://placehold.co/240x320/{background.TrimStart('#')}/ffffff.png?text={Uri.EscapeDataString(title)}";

    private sealed record SeedBook(string Id, string Name, string Author);
}
