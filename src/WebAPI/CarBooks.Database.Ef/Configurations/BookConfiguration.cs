using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBooks.Database.Ef.Configurations;

internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Name)
            .HasMaxLength(CatalogConsts.MaxBookNameLength)
            .IsRequired();

        builder.Property(book => book.Author)
            .HasMaxLength(CatalogConsts.MaxBookAuthorLength)
            .IsRequired();

        builder.Property(book => book.CoverUrl)
            .HasMaxLength(CatalogConsts.MaxCoverUrlLength)
            .IsRequired();

        builder.Property(book => book.CoverImage)
            .HasColumnType("bytea");

        builder.Property(book => book.CoverImageContentType)
            .HasMaxLength(CatalogConsts.MaxContentTypeLength);

        builder.Property(book => book.DisplayOrder)
            .IsRequired();

        builder.Ignore(book => book.HasCoverImage);

        builder.HasIndex(book => new { book.CategoryId, book.DisplayOrder });
    }
}
