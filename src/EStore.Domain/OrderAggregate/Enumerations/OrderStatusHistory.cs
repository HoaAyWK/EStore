using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.OrderAggregate.Enumerations;

public sealed class OrderStatusHistory : Enumeration<OrderStatusHistory>
{
    public static readonly OrderStatusHistory OrderPlaced = new(1, nameof(OrderPlaced));

    public static readonly OrderStatusHistory PaymentInfoConfirmed = new(2, nameof(PaymentInfoConfirmed));

    public static readonly OrderStatusHistory OrderShippedOut = new(3, nameof(OrderShippedOut));

    public static readonly OrderStatusHistory OrderReceived = new(4, nameof(OrderReceived));

    public static readonly OrderStatusHistory OrderCompleted = new(5, nameof(OrderCompleted));

    private OrderStatusHistory(int value, string name)
        : base(value, name)
    {
    }
}
