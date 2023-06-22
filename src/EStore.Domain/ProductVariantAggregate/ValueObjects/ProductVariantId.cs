using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductVariantAggregate.ValueObjects;

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

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
