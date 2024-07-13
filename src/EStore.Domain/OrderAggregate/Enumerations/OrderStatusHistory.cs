using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.OrderAggregate.Enumerations;

public sealed class OrderStatusHistory : Enumeration<OrderStatusHistory>
{
    public static readonly OrderStatusHistory OrderPlaced = new(1, nameof(OrderPlaced));

    public static readonly OrderStatusHistory PaymentInfoConfirmed = new(2, nameof(PaymentInfoConfirmed));

    public static readonly OrderStatusHistory OrderShippedOut = new(3, nameof(OrderShippedOut));

    public static readonly OrderStatusHistory InTransit = new(4, nameof(InTransit));

    public static readonly OrderStatusHistory OrderReceived = new(5, nameof(OrderReceived));

    public static readonly OrderStatusHistory OrderCompleted = new(6, nameof(OrderCompleted));

    public static readonly OrderStatusHistory OrderCancelled = new(7, nameof(OrderCancelled));

    public static readonly OrderStatusHistory OrderRefunded = new(8, nameof(OrderRefunded));

    private OrderStatusHistory(int value, string name)
        : base(value, name)
    {
    }
}
