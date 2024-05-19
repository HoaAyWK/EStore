using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Orders.Services;

public interface IOrderReadService
{
    Task<PagedList<OrderResponse>> GetListPagedAsync(
        int page,
        int pageSize,
        string? orderStatus,
        string? order,
        string? orderBy,
        int? orderNumber);

    Task<OrderResponse?> GetByIdAsync(OrderId orderId);
    Task<PagedList<Order>> GetByCustomerIdAsync(
        CustomerId customerId,
        int page,
        int pageSize,
        OrderStatus? orderStatus = null,
        CancellationToken cancellationToken = default);

    Task<List<OrderResponse>> GetOrdersByCriteriaAsync(
        CustomerId customerId,
        ProductId productId,
        ProductVariantId? productVariantId);

    Task<OrderStats> GetOrderStatsAsync(int lastDaysCount);

    Task<IncomeStats> GetIncomeStatsAsync(int lastDaysCount);
}
