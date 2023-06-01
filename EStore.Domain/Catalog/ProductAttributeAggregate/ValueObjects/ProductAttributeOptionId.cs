using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;

public sealed class ProductAttributeOptionId : ValueObject
{
    public Guid Value { get; }

    private ProductAttributeOptionId(Guid value)
    {
        Value = value;
    }

    public static ProductAttributeOptionId Create(Guid value)
    {
        return new(value);
    }

    public static ProductAttributeOptionId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
