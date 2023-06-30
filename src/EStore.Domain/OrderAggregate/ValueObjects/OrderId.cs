using EStore.Domain.Common.Models;

namespace EStore.Domain.OrderAggregate.ValueObjects;

public sealed class OrderId : ValueObject
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    public static OrderId Create(Guid value)
    {
        return new(value);
    }

    public static OrderId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
