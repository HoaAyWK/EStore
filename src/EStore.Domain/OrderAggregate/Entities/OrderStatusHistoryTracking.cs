using EStore.Domain.Common.Models;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate.Entities;

public class OrderStatusHistoryTracking : Entity<OrderStatusHistoryTrackingId>
{
    public OrderStatusHistory Status { get; private set; } = OrderStatusHistory.OrderPlaced;

    public DateTime CreatedDateTime { get; private set; }

    private OrderStatusHistoryTracking()
    {
    }

    private OrderStatusHistoryTracking(
        OrderStatusHistoryTrackingId id,
        OrderStatusHistory status,
        DateTime createdDateTime)
        : base(id)
    {
        Status = status;
        CreatedDateTime = createdDateTime;
    }

    public static OrderStatusHistoryTracking Create(
        OrderStatusHistory status,
        DateTime createdDateTime)
    {
        return new(
            OrderStatusHistoryTrackingId.CreateUnique(),
            status,
            createdDateTime);
    }
}
