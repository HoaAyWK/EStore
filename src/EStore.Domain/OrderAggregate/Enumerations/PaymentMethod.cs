using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.OrderAggregate.Enumerations;

public class PaymentMethod : Enumeration<PaymentMethod>
{
    public static readonly PaymentMethod CashOnDelivery = new(1, nameof(CashOnDelivery));

    public static readonly PaymentMethod CreditCard = new(2, nameof(CreditCard));

    private PaymentMethod(int value, string name)
        : base(value, name)
    {
    }
}
