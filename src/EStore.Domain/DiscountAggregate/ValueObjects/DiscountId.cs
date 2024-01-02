using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.DiscountAggregate.ValueObjects;

public sealed class DiscountId : ValueObject
{
    public Guid Value { get; }

    private DiscountId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private DiscountId(string value)
    {
        Value = new Guid(value);
    }

    public static DiscountId Create(Guid value)
    {
        return new(value);
    }

    public static DiscountId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
