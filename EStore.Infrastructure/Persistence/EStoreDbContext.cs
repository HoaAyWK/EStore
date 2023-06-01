using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate;
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
    }

    public DbSet<Category> Categories { get; set; } = null!;    

    public DbSet<Brand> Brands { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<ProductAttribute> ProductAttributes { get; set; } = null!;

}
