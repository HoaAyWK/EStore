using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.OrderAggregate.Enumerations;

public sealed class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(1, nameof(Pending));

    public static readonly OrderStatus Processing = new(2, nameof(Processing));

    public static readonly OrderStatus Completed = new(3, nameof(Completed));

    public static readonly OrderStatus Cancelled = new(4, nameof(Cancelled));

    public static readonly OrderStatus Refunded = new(5, nameof(Refunded));

    private OrderStatus(int value, string name)
        : base(value, name)
    {
    }
}
