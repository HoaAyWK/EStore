using EStore.Contracts.Discount;
using EStore.Contracts.Discounts;
using EStore.Domain.DiscountAggregate.ValueObjects;

namespace EStore.Application.Discounts.Services;

public interface IDiscountReadService
{
    Task<DiscountResponse?> GetDiscountByIdAsync(DiscountId discountId);
    Task<ListPagedDiscountResponse> GetListPagedAsync(int pageSize, int page);
}
