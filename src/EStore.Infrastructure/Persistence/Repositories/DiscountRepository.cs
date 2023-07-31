using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.DiscountAggregate.ValueObjects;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class DiscountRepository : IDiscountRepository
{
    private readonly EStoreDbContext _dbContext;

    public DiscountRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Discount discount)
    {
        await _dbContext.Discounts.AddAsync(discount);
    }

    public async Task<Discount?> GetByIdAsync(DiscountId discountId)
    {
        return await _dbContext.Discounts.FindAsync(discountId);
    }

    public void Delete(Discount discount)
    {
        _dbContext.Discounts.Remove(discount);
    } 
}
