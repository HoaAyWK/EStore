using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.OrderAggregate.ValueObjects;

public sealed class OrderStatusHistoryTrackingId : ValueObject
{
    public Guid Value { get; }

    private OrderStatusHistoryTrackingId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private OrderStatusHistoryTrackingId(string value)
    {
        Value = new Guid(value);
    }

    public static OrderStatusHistoryTrackingId Create(Guid value)
    {
        return new(value);
    }

    public static OrderStatusHistoryTrackingId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
