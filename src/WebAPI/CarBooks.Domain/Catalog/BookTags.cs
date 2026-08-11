using System.ComponentModel.DataAnnotations;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Catalog;

/// <summary>
/// Many-to-many link between a <see cref="Book"/> and a <see cref="Tag"/>.
/// The pair (<see cref="TagId"/>, <see cref="BookId"/>) is unique.
/// </summary>
public sealed class BookTags
{
    public BookTags(Guid tagId, Guid bookId)
    {
        if (tagId == Guid.Empty)
        {
            throw new DomainValidationException("Tag id must not be empty.");
        }

        if (bookId == Guid.Empty)
        {
            throw new DomainValidationException("Book id must not be empty.");
        }

        TagId = tagId;
        BookId = bookId;
    }

    private BookTags()
    {
    }

    [Required]
    public Guid TagId { get; private set; }

    [Required]
    public Guid BookId { get; private set; }
}
