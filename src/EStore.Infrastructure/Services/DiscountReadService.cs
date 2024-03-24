using EStore.Application.Discounts.Services;
using EStore.Contracts.Discount;
using EStore.Contracts.Discounts;
using EStore.Domain.DiscountAggregate.ValueObjects;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

internal sealed class DiscountReadService : IDiscountReadService
{
    private readonly EStoreDbContext _dbContext;

    public DiscountReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DiscountResponse?> GetDiscountByIdAsync(DiscountId discountId)
    {
        return await _dbContext.Discounts.AsNoTracking()
            .Where(d => d.Id == discountId)
            .Select(discount => new DiscountResponse(
                discount.Id.Value,
                discount.Name,
                discount.UsePercentage,
                discount.DiscountPercentage,
                discount.DiscountAmount,
                discount.StartDateTime,
                discount.EndDateTime,
                discount.CreatedDateTime,
                discount.UpdatedDateTime))
            .FirstOrDefaultAsync();
    }

    public async Task<ListPagedDiscountResponse> GetListPagedAsync(int pageSize, int page)
    {
        IQueryable<DiscountResponse> discountResponseQuery = 
            from discount in _dbContext.Discounts.AsNoTracking()
            select new DiscountResponse(
                discount.Id.Value,
                discount.Name,
                discount.UsePercentage,
                discount.DiscountPercentage,
                discount.DiscountAmount,
                discount.StartDateTime,
                discount.EndDateTime,
                discount.CreatedDateTime,
                discount.UpdatedDateTime);

        int totalItems = await discountResponseQuery.CountAsync();

        if (pageSize is 0 || page is 0)
        {
            return new ListPagedDiscountResponse()
            {
                Items = await discountResponseQuery.ToListAsync(),
                Page = 1,
                PageSize = totalItems,
                TotalItems = totalItems
            };
        }

        var discountResponsesListPaged = await discountResponseQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ListPagedDiscountResponse()
        {
            Items = discountResponsesListPaged,
            PageSize = pageSize,
            Page = page,
            TotalItems = totalItems
        };
    }
}
