using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public class ProductAttributeId : ValueObject
{
    public Guid Value { get; }

    private ProductAttributeId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    public ProductAttributeId(string value)
    {
        Value = new Guid(value);
    }

    public static ProductAttributeId Create(Guid value)
    {
        return new(value);
    }

    public static ProductAttributeId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
