using EStore.Domain.BrandAggregate;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence;

public class EStoreDbContext : DbContext
{
    public EStoreDbContext(DbContextOptions<EStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(EStoreDbContext).Assembly);

        modelBuilder.Entity<Product>()
            .Ignore("ProductVariantAttributeCombinations.AttributeSelection")
            .Ignore("ProductAttributes.ProductAttributeValues.ConnectedAttributes");
    }

    public DbSet<Category> Categories { get; set; } = null!;    

    public DbSet<Brand> Brands { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

}
