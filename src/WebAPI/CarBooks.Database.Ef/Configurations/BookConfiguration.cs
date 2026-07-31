using CarBooks.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        builder.Property(book => book.CoverUrl).IsRequired();
        builder.Property(book => book.CoverImage).HasColumnType("bytea");

        builder.Ignore(book => book.HasCoverImage);

        builder.HasMany(book => book.Categories)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "BookCategories",
                join => join
                    .HasOne<Category>()
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade),
                join => join
                    .HasOne<Book>()
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("BookId", "CategoryId");
                    join.ToTable("BookCategories");
                });

        builder.Navigation(book => book.Categories)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
