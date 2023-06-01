namespace EStore.Domain.Catalog.ProductAggregate.ValueObjects;

using EStore.Domain.Common.Models;

public class ProductVariantAttributeId : ValueObject
{
    public Guid Value { get; }

    private ProductVariantAttributeId(Guid value)
    {
        Value = value;
    }

    public static ProductVariantAttributeId Create(Guid value)
    {
        return new(value);
    }

    public static ProductVariantAttributeId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
