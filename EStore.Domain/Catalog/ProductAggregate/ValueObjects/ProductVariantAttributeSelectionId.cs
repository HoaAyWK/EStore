using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate.ValueObjects;

public sealed class ProductVariantAttributeSelectionId : ValueObject
{
    public Guid Value { get; }

    private ProductVariantAttributeSelectionId(Guid value)
    {
        Value = value;
    }

    public static ProductVariantAttributeSelectionId Create(Guid value)
    {
        return new(value);
    }

    public static ProductVariantAttributeSelectionId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
