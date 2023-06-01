using EStore.Domain.Catalog.ProductAttributeAggregate;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductAttributeRepository : IProductAttributeRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductAttributeRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ProductAttribute productAttribute)
    {
        await _dbContext.ProductAttributes.AddAsync(productAttribute);
    }

    public void Delete(ProductAttribute productAttribute)
    {
        _dbContext.ProductAttributes.Remove(productAttribute);
    }
}
