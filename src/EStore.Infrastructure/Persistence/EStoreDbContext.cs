using EStore.Domain.BrandAggregate;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.ProductAggregate;
using EStore.Domain.UserAggregate;
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
        
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EStoreDbContext).Assembly);
    }

    public DbSet<Category> Categories { get; set; } = null!;    

    public DbSet<Brand> Brands { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

}
