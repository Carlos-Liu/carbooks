using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Many-to-many link between a <see cref="Category"/> and a <see cref="Book"/>.
/// The pair (<see cref="CategoryId"/>, <see cref="BookId"/>) is unique.
/// </summary>
public sealed class CategoryBooks
{
    public CategoryBooks(Guid categoryId, Guid bookId)
    {
        if (categoryId == Guid.Empty)
        {
            throw new DomainValidationException("Category id must not be empty.");
        }

        if (bookId == Guid.Empty)
        {
            throw new DomainValidationException("Book id must not be empty.");
        }

        CategoryId = categoryId;
        BookId = bookId;
    }

    private CategoryBooks()
    {
    }

    [Required]
    public Guid CategoryId { get; private set; }

    [Required]
    public Guid BookId { get; private set; }
}
