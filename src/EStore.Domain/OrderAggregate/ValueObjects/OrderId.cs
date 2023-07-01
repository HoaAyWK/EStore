using EStore.Domain.Common.Models;
using Newtonsoft.Json;

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

    [JsonConstructor]
    private OrderId(string value)
    {
        Value = new Guid(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
