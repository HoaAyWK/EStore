using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.BrandAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class BrandReadRepository : IBrandReadRepository
{
    private readonly EStoreDbContext _dbContext;

    public BrandReadRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Brand>> GetAllAsync()
    {
        return await _dbContext.Brands.AsNoTracking()
            .ToListAsync();
    }

    public async Task<Brand?> GetByIdAsync(BrandId brandId)
    {
        return await _dbContext.Brands.AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == brandId);
    }
}
