namespace CarBooks.Domain.Catalog;

/// <summary>A tag paired with the books linked to it.</summary>
public sealed record TagWithBooks(Tag Tag, IReadOnlyList<Book> Books);
