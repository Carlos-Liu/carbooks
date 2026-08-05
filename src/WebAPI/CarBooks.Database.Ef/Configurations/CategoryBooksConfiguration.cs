using CarBooks.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBooks.Database.Ef.Configurations;

internal sealed class CategoryBooksConfiguration : IEntityTypeConfiguration<CategoryBooks>
{
    public void Configure(EntityTypeBuilder<CategoryBooks> builder)
    {
        builder.ToTable("CategoryBooks");

        builder.HasKey(link => new { link.CategoryId, link.BookId });

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(link => link.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Book>()
            .WithMany()
            .HasForeignKey(link => link.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(link => link.BookId);
    }
}
