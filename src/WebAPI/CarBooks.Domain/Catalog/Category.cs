using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Aggregate root grouping the books shown on a single catalog page.
/// </summary>
public sealed class Category : Entity
{
    private readonly List<Book> books = [];

    public Category(Guid id, string name, string slug, int displayOrder)
        : base(id)
    {
        Name = Guard.Text(name, nameof(Name), CatalogConsts.MaxCategoryNameLength);
        Slug = NormaliseSlug(slug);
        DisplayOrder = Guard.NotNegative(displayOrder, nameof(DisplayOrder));
    }

    private Category()
    {
    }

    public string Name { get; private set; } = string.Empty;

    /// <summary>URL-safe identifier used by the SPA route, for example <c>category-1</c>.</summary>
    public string Slug { get; private set; } = string.Empty;

    public int DisplayOrder { get; private set; }

    public IReadOnlyList<Book> Books => books;

    public Book AddBook(Guid bookId, string name, string author, string coverUrl, int displayOrder)
    {
        if (books.Any(b => string.Equals(b.Name, name?.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new DomainValidationException($"Category '{Name}' already contains a book named '{name}'.");
        }

        var book = new Book(bookId, Id, name!, author, coverUrl, displayOrder);
        books.Add(book);
        return book;
    }

    public void Rename(string name) =>
        Name = Guard.Text(name, nameof(Name), CatalogConsts.MaxCategoryNameLength);

    private static string NormaliseSlug(string? slug)
    {
        var trimmed = Guard.Text(slug, nameof(Slug), CatalogConsts.MaxCategorySlugLength).ToLowerInvariant();
        if (!trimmed.All(c => char.IsAsciiLetterOrDigit(c) || c == '-'))
        {
            throw new DomainValidationException("Slug may only contain letters, digits and hyphens.");
        }

        return trimmed;
    }
}
