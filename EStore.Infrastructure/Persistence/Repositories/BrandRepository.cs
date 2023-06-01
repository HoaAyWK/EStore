using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.BrandAggregate.ValueObjects;

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
}
