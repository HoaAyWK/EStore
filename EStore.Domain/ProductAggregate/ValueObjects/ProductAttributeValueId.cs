using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public sealed class ProductAttributeValueId : ValueObject
{
    public Guid Value { get; }

    private ProductAttributeValueId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    public ProductAttributeValueId(string value)
    {
        Value = new Guid(value);
    }

    public static ProductAttributeValueId Create(Guid value)
    {
        return new(value);
    }

    public static ProductAttributeValueId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static bool CanParse(string value)
    {
        if (Guid.TryParse(value, out _))
        {
            return true;
        }

        return false;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
