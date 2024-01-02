using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Brands;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

internal sealed class BrandReadService : IBrandReadService
{
    private readonly EStoreDbContext _dbContext;

    public BrandReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BrandResponse?> GetByIdAsync(BrandId brandId)
    {
        return await _dbContext.Brands.AsNoTracking()
            .Where(b => b.Id == brandId)
            .Select(b => new BrandResponse(
                b.Id.Value,
                b.Name,
                b.CreatedDateTime,
                b.UpdatedDateTime))
            .FirstOrDefaultAsync();
    }

    public async Task<ListPagedBrandResponse> GetListPagedAsync(int pageSize = 0, int page = 0)
    {
        IQueryable<BrandResponse> brandResponsesQuery = 
            from brand in _dbContext.Brands.AsNoTracking()
            select new BrandResponse(
                brand.Id.Value,
                brand.Name,
                brand.CreatedDateTime,
                brand.UpdatedDateTime);

        int totalItems = await brandResponsesQuery.CountAsync();

        if (pageSize is 0 || page is 0)
        {
            return new ListPagedBrandResponse()
            {
                Data = await brandResponsesQuery.ToListAsync(),
                Page = 1,
                PageSize = totalItems,
                TotalItems = totalItems
            };
        }

        var brandResponsesListPaged = await brandResponsesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ListPagedBrandResponse()
        {
            Data = brandResponsesListPaged,
            PageSize = pageSize,
            Page = page,
            TotalItems = totalItems
        };
    }
}
