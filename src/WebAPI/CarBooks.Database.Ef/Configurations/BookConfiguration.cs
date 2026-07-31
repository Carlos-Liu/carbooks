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

        builder.HasIndex(book => book.DisplayOrder);

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
