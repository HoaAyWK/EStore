using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public sealed class ProductVariantId : ValueObject
{
    public Guid Value { get; }

    private ProductVariantId(Guid value)
    {
        Value = value;
    }

    public static ProductVariantId Create(Guid value)
    {
        return new(value);
    }

    public static ProductVariantId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    [JsonConstructor]
    private ProductVariantId(string value)
    {
        Value = new Guid(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
