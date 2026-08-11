using CarBooks.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBooks.Database.Ef.Configurations;

internal sealed class BookTagsConfiguration : IEntityTypeConfiguration<BookTags>
{
    public void Configure(EntityTypeBuilder<BookTags> builder)
    {
        builder.ToTable("BookTags");

        builder.HasKey(link => new { link.TagId, link.BookId });

        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(link => link.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Book>()
            .WithMany()
            .HasForeignKey(link => link.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(link => link.BookId);
    }
}
