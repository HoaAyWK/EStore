using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductReadRepository : IProductReadRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductReadRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetByIdAsync(ProductId productId)
    {
        return await _dbContext.Products.FindAsync(productId);
    }
}
