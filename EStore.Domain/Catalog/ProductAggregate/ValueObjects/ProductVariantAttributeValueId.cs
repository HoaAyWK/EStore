using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate.ValueObjects;

public sealed class ProductVariantAttributeValueId : ValueObject
{
    public Guid Value { get; }

    private ProductVariantAttributeValueId(Guid value)
    {
        Value = value;
    }

    public static ProductVariantAttributeValueId Create(Guid value)
    {
        return new(value);
    }

    public static ProductVariantAttributeValueId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
