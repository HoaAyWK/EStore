using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.OrderAggregate.Enumerations;

public class PaymentStatus : Enumeration<PaymentStatus>
{
    public static readonly PaymentStatus Pending = new(1, nameof(Pending));

    public static readonly PaymentStatus Paid = new(2, nameof(Paid));

    public static readonly PaymentStatus Failed = new(3, nameof(Failed));

    private PaymentStatus(int value, string name)
        : base(value, name)
    {
    }
}
