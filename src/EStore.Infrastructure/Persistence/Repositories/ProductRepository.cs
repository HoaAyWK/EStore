using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly EStoreDbContext _dbContext;

    public ProductRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetByIdAsync(ProductId productId)
        => await _dbContext.Products.FindAsync(productId);

    public async Task AddAsync(Product product)
        => await _dbContext.Products.AddAsync(product);

    public async Task<bool> AnyProductWithCategoryId(CategoryId categoryId)
    {
        return await _dbContext.Products.AnyAsync(
            p => p.CategoryId == categoryId);
    }

    public async Task<bool> AnyProductWithBrandId(BrandId brandId)
    {
        return await _dbContext.Products.AnyAsync(
            p => p.BrandId == brandId);
    }
}
