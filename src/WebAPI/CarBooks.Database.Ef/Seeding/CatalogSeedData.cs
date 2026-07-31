using CarBooks.Domain.Catalog;

namespace CarBooks.Database.Ef.Seeding;

/// <summary>
/// The starter catalog: three categories with three books each. Identifiers are fixed so repeated
/// seeding across environments produces the same rows.
/// </summary>
internal static class CatalogSeedData
{
    public static (IReadOnlyList<Category> Categories, IReadOnlyList<Book> Books) CreateCatalog()
    {
        var category1 = new Category(
            Guid.Parse("11111111-1111-4111-8111-111111110001"),
            "Category 1");
        var category2 = new Category(
            Guid.Parse("11111111-1111-4111-8111-111111110002"),
            "Category 2");
        var category3 = new Category(
            Guid.Parse("11111111-1111-4111-8111-111111110003"),
            "Category 3");

        var books = new List<Book>
        {
            CreateBook(
                "22222222-2222-4222-8222-222222220001",
                "Go Like Hell",
                "A. J. Baime",
                topColor: "#334155",
                bottomColor: "#0f172a",
                category1),
            CreateBook(
                "22222222-2222-4222-8222-222222220002",
                "The Limit",
                "Michael Cannell",
                topColor: "#334155",
                bottomColor: "#0f172a",
                category1),
            CreateBook(
                "22222222-2222-4222-8222-222222220003",
                "Ferrari Rex",
                "Luca Dal Monte",
                topColor: "#334155",
                bottomColor: "#0f172a",
                category1),
            CreateBook(
                "22222222-2222-4222-8222-222222220004",
                "How to Build a Car",
                "Adrian Newey",
                topColor: "#9f1239",
                bottomColor: "#4c0519",
                category2),
            CreateBook(
                "22222222-2222-4222-8222-222222220005",
                "Total Competition",
                "Ross Brawn and Adam Parr",
                topColor: "#9f1239",
                bottomColor: "#4c0519",
                category2),
            CreateBook(
                "22222222-2222-4222-8222-222222220006",
                "The Mechanic",
                "Marc Priestley",
                topColor: "#9f1239",
                bottomColor: "#4c0519",
                category2),
            CreateBook(
                "22222222-2222-4222-8222-222222220007",
                "Faster",
                "Neal Bascomb",
                topColor: "#166534",
                bottomColor: "#052e16",
                category3),
            CreateBook(
                "22222222-2222-4222-8222-222222220008",
                "The Art of the Formula 1 Race Car",
                "Stuart Codling",
                topColor: "#166534",
                bottomColor: "#052e16",
                category3),
            CreateBook(
                "22222222-2222-4222-8222-222222220009",
                "Car Guys vs. Bean Counters",
                "Bob Lutz",
                topColor: "#166534",
                bottomColor: "#052e16",
                category3),
        };

        return ([category1, category2, category3], books);
    }

    private static Book CreateBook(
        string id,
        string name,
        string author,
        string topColor,
        string bottomColor,
        params Category[] categories)
    {
        var book = new Book(
            Guid.Parse(id),
            name,
            author,
            BuildCoverUrl(name, topColor));

        foreach (var category in categories)
        {
            book.AssignToCategory(category);
        }

        book.SetCoverImage(
            PlaceholderCover.Create(name, author, topColor, bottomColor),
            PlaceholderCover.ContentType);

        return book;
    }

    private static string BuildCoverUrl(string title, string background) =>
        $"https://placehold.co/240x320/{background.TrimStart('#')}/ffffff.png?text={Uri.EscapeDataString(title)}";
}
