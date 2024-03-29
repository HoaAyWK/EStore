using EStore.Domain.BrandAggregate;
using EStore.Domain.CartAggregate;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.OrderAggregate;
using EStore.Domain.ProductAggregate;
using EStore.Infrastructure.Identity;
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
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(
            typeof(EStoreDbContext).Assembly);
    }

    public DbSet<Category> Categories { get; set; } = null!;    

    public DbSet<Brand> Brands { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Customer> Customers { get; set; } = null!;

    public DbSet<Cart> Carts { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<Discount> Discounts { get; set; } = null!;

    public DbSet<AccountToken> AccountTokens { get; set; } = null!;
}
