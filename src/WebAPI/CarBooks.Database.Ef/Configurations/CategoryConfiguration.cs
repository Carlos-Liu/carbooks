using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBooks.Database.Ef.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(CatalogConsts.MaxCategoryNameLength)
            .IsRequired();

        builder.Property(category => category.Slug)
            .HasMaxLength(CatalogConsts.MaxCategorySlugLength)
            .IsRequired();

        builder.Property(category => category.DisplayOrder)
            .IsRequired();

        builder.HasIndex(category => category.Slug)
            .IsUnique();

        builder.HasMany(category => category.Books)
            .WithOne(book => book.Category)
            .HasForeignKey(book => book.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // The collection is exposed read-only, so EF Core must go through the backing field.
        builder.Navigation(category => category.Books)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
