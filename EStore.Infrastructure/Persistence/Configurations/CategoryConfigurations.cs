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
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
        
        CategoryId? defaultCategoryId = null;

        builder.Property(c => c.ParentId)
            .HasColumnName("ParentCategoryId")
            .ValueGeneratedNever()
            .HasConversion(
                parentId => parentId != defaultCategoryId ? parentId!.Value : Guid.Empty,
                value =>  value != Guid.Empty ? CategoryId.Create(value) : null);

        builder.Property(c => c.Name)
            .HasMaxLength(100);

        builder.Property(c => c.Id)
            .HasColumnName("CategoryId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CategoryId.Create(value));
    }
}
