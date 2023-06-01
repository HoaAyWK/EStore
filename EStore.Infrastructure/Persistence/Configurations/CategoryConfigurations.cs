using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
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
        
        builder.Property(c => c.Name)
            .HasMaxLength(200);

        builder.Property(c => c.Id)
            .HasColumnName("CategoryId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CategoryId.Create(value));
    }
}
