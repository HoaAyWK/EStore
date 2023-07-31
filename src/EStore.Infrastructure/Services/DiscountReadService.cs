using EStore.Application.Discounts.Services;
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
                discount.EndDateTime))
            .FirstOrDefaultAsync();
    }
}
