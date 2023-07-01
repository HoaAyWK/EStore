using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.CustomerAggregate.ValueObjects;

public sealed class CustomerId : ValueObject
{
    public Guid Value { get; }

    private CustomerId(Guid value)
    {
        Value = value;
    }
    
    [JsonConstructor]
    private CustomerId(string value)
    {
        Value = new Guid(value);
    }

    public static CustomerId Create(Guid value)
    {
        return new(value);
    }

    public static CustomerId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
