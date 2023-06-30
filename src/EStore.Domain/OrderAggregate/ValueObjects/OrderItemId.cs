using EStore.Domain.Common.Models;

namespace EStore.Domain.OrderAggregate.ValueObjects;

public sealed class OrderItemId : ValueObject
{
    public Guid Value { get; }

    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public static OrderItemId Create(Guid value)
    {
        return new(value);
    }

    public static OrderItemId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
