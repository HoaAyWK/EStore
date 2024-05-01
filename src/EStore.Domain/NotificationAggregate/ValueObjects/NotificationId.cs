using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.NotificationAggregate.ValueObjects;

public sealed class NotificationId : ValueObject
{
    public Guid Value { get; }

    private NotificationId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private NotificationId(string value)
    {
        Value = new Guid(value);
    }

    public static NotificationId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static NotificationId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
