using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class BrandConfigurations : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        ConfigureBrandTable(builder);
    }

    private void ConfigureBrandTable(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Name)
            .HasMaxLength(100);

        builder.Property(b => b.Id)
            .HasColumnName("BrandId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BrandId.Create(value));
    }
}
