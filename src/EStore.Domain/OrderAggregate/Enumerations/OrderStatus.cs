using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.OrderAggregate.Enumerations;

public sealed class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new OrderStatus(1, nameof(Pending));

    public static readonly OrderStatus Processing = new OrderStatus(2, nameof(Processing));

    public static readonly OrderStatus Paid = new OrderStatus(3, nameof(Paid));

    public static readonly OrderStatus Cancelled = new OrderStatus(4, nameof(Cancelled));

    private OrderStatus(int value, string name)
        : base(value, name)
    {
    }
}
