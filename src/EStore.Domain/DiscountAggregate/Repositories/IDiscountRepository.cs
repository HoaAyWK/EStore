using EStore.Domain.DiscountAggregate.ValueObjects;

namespace EStore.Domain.DiscountAggregate.Repositories;

public interface IDiscountRepository
{
    Task AddAsync(Discount discount);
    Task<Discount?> GetByIdAsync(DiscountId discountId);
    void Delete(Discount discount);
}
