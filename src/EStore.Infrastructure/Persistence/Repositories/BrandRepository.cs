using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.BrandAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal class BrandRepository :  IBrandRepository
{
    private readonly EStoreDbContext _dbContext;

    public BrandRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Brand brand)
    {
        await _dbContext.Brands.AddAsync(brand);
    }

    public async Task<Brand?> GetByIdAsync(BrandId id)
    {
        return await _dbContext.Brands.FindAsync(id);
    }

    public async Task<Brand?> GetByNameAsync(string name)
    {
        return await _dbContext.Brands
            .Where(brand => brand.Name == name)
            .FirstOrDefaultAsync();
    }

    public void Delete(Brand brand)
    {
        _dbContext.Remove(brand);
    }
}
