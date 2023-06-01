using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate.ValueObjects;

public sealed class ProductVariantAttributeCombinationId : ValueObject
{
    public Guid Value { get; }

    private ProductVariantAttributeCombinationId(Guid value)
    {
        Value = value;
    }

    public static ProductVariantAttributeCombinationId Create(Guid value)
    {
        return new(value);
    }

    public static ProductVariantAttributeCombinationId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
