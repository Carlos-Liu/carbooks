using CarBooks.Domain.Catalog;
using CarBooks.Domain.Shared;
using Microsoft.EntityFrameworkCore;
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

        builder.Property(category => category.DisplayOrder)
            .IsRequired();
    }
}
