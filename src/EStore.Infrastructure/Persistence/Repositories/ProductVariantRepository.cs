using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate;
using EStore.Domain.ProductVariantAggregate.Repositories;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductVariantRepository : IProductVariantRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductVariantRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ProductVariant productVariant)
    {
        await _dbContext.ProductVariants.AddAsync(productVariant);
    }

    public async Task<List<ProductVariant>> GetAllAsync()
    {
        return await _dbContext.ProductVariants.ToListAsync();
    }

    public async Task<ProductVariant?> GetByIdAsync(ProductVariantId id)
    {
        return await _dbContext.ProductVariants.FindAsync(id);
    }

    public async Task<List<ProductVariant>> GetByProductIdAsync(ProductId productId)
    {
        return await _dbContext.ProductVariants.Where(v => v.ProductId == productId)
            .ToListAsync();
    }
}
