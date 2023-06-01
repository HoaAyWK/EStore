using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Product product)
        => await _dbContext.Products.AddAsync(product);

    public async Task<Product?> GetByIdAsync(ProductId id)
        => await _dbContext.Products.FindAsync(id);
}
