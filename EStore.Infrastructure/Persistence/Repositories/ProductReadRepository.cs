using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductReadRepository : IProductReadRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductReadRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AnyProductWithBrandId(BrandId brandId)
    {
        return await _dbContext.Products.AnyAsync(x => x.BrandId == brandId);
    }

    public async Task<bool> AnyProductWithCategoryId(CategoryId categoryId)
    {
        return await _dbContext.Products.AnyAsync(x => x.CategoryId == categoryId);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllWithBrandAndCategory()
    {
        return await _dbContext.Products.AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(ProductId productId)
    {
        return await _dbContext.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);
    }
}
