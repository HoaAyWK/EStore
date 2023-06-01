using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductAttributeReadRepository : IProductAttributeReadRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductAttributeReadRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductAttribute>> GetAllAsync()
    {
        return await _dbContext.ProductAttributes.ToListAsync();
    }

    public async Task<ProductAttribute?> GetByIdAsync(ProductAttributeId productAttributeId)
    {
        return await _dbContext.ProductAttributes.FindAsync(productAttributeId);
    }
}
