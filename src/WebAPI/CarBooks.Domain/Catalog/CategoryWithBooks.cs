namespace CarBooks.Domain.Catalog;

/// <summary>A category paired with the books linked to it.</summary>
public sealed record CategoryWithBooks(Category Category, IReadOnlyList<Book> Books);
