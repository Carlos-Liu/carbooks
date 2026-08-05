using CarBooks.Domain.Catalog;

namespace CarBooks.Database.Ef.Seeding;

/// <summary>
/// The starter catalog: three categories with three books each. Identifiers are fixed so repeated
/// seeding across environments produces the same rows.
/// </summary>
internal static class CatalogSeedData
{
    public static (
        IReadOnlyList<Category> Categories,
        IReadOnlyList<Book> Books,
        IReadOnlyList<CategoryBooks> Links) CreateCatalog()
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

        var book1 = CreateBook("22222222-2222-4222-8222-222222220001", "Go Like Hell", "A. J. Baime", "#334155", "#0f172a");
        var book2 = CreateBook("22222222-2222-4222-8222-222222220002", "The Limit", "Michael Cannell", "#334155", "#0f172a");
        var book3 = CreateBook("22222222-2222-4222-8222-222222220003", "Ferrari Rex", "Luca Dal Monte", "#334155", "#0f172a");
        var book4 = CreateBook("22222222-2222-4222-8222-222222220004", "How to Build a Car", "Adrian Newey", "#9f1239", "#4c0519");
        var book5 = CreateBook("22222222-2222-4222-8222-222222220005", "Total Competition", "Ross Brawn and Adam Parr", "#9f1239", "#4c0519");
        var book6 = CreateBook("22222222-2222-4222-8222-222222220006", "The Mechanic", "Marc Priestley", "#9f1239", "#4c0519");
        var book7 = CreateBook("22222222-2222-4222-8222-222222220007", "Faster", "Neal Bascomb", "#166534", "#052e16");
        var book8 = CreateBook("22222222-2222-4222-8222-222222220008", "The Art of the Formula 1 Race Car", "Stuart Codling", "#166534", "#052e16");
        var book9 = CreateBook("22222222-2222-4222-8222-222222220009", "Car Guys vs. Bean Counters", "Bob Lutz", "#166534", "#052e16");

        IReadOnlyList<CategoryBooks> links =
        [
            new CategoryBooks(category1.Id, book1.Id),
            new CategoryBooks(category1.Id, book2.Id),
            new CategoryBooks(category1.Id, book3.Id),
            new CategoryBooks(category2.Id, book4.Id),
            new CategoryBooks(category2.Id, book5.Id),
            new CategoryBooks(category2.Id, book6.Id),
            new CategoryBooks(category3.Id, book7.Id),
            new CategoryBooks(category3.Id, book8.Id),
            new CategoryBooks(category3.Id, book9.Id),
        ];

        return (
            [category1, category2, category3],
            [book1, book2, book3, book4, book5, book6, book7, book8, book9],
            links);
    }

    private static Book CreateBook(
        string id,
        string name,
        string author,
        string topColor,
        string bottomColor)
    {
        var book = new Book(
            Guid.Parse(id),
            name,
            author,
            BuildCoverUrl(name, topColor));

        book.SetCoverImage(
            PlaceholderCover.Create(name, author, topColor, bottomColor),
            PlaceholderCover.ContentType);

        return book;
    }

    private static string BuildCoverUrl(string title, string background) =>
        $"https://placehold.co/240x320/{background.TrimStart('#')}/ffffff.png?text={Uri.EscapeDataString(title)}";
}
