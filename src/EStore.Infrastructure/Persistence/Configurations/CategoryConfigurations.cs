using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        ConfigureCategoryTable(builder);
    }

    private void ConfigureCategoryTable(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(c => c.ParentId)
            .HasColumnName("ParentCategoryId")
            .ValueGeneratedNever()
            .HasConversion(
                parentId => parentId!.Value,
                value =>  CategoryId.Create(value));

        builder.Property(c => c.Name)
            .HasMaxLength(Category.MaxNameLength);

        builder.Property(c => c.Slug)
            .HasMaxLength(Category.MaxSlugLength);

        builder.Property(c => c.Id)
            .HasColumnName("CategoryId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CategoryId.Create(value));

        builder.HasIndex(c => c.Slug);

        builder.HasIndex(c => c.Name);

        builder.HasQueryFilter(c => !c.Deleted);
    }
}
