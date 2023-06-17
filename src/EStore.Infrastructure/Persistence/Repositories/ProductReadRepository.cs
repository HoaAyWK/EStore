using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductReadRepository : IProductReadRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductReadRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllWithBrandAndCategory()
    {
        return await _dbContext.Products.AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(ProductId productId)
    {
        return await _dbContext.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);
    }
}
