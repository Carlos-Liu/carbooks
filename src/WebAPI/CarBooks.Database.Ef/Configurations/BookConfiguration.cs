using CarBooks.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBooks.Database.Ef.Configurations;

internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Name).IsRequired();
        builder.Property(book => book.Author).IsRequired();
        builder.Property(book => book.CoverImage).HasColumnType("bytea");
        builder.Property(book => book.PublishedOn).HasColumnType("date");

        builder.Ignore(book => book.HasCoverImage);
    }
}
