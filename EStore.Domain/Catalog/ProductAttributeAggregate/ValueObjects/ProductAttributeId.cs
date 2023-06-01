using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;

public sealed class ProductAttributeId : ValueObject
{
    public Guid Value { get; }

    private ProductAttributeId(Guid value)
    {
        Value = value;
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
