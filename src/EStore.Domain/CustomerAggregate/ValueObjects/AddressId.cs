using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.CustomerAggregate.ValueObjects;

public class AddressId : ValueObject
{
    public Guid Value { get; set; }

    private AddressId(Guid value)
    {
        Value = value;
    }
    
    [JsonConstructor]
    private AddressId(string value)
    {
        Value = new Guid(value);
    }

    public static AddressId Create(Guid value)
    {
        return new(value);
    }

    public static AddressId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
