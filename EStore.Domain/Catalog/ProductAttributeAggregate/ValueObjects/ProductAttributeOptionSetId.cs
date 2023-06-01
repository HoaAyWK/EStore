using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;

public sealed class ProductAttributeOptionSetId : ValueObject
{
    public Guid Value { get; }

    private ProductAttributeOptionSetId(Guid value)
    {
        Value = value;
    }

    public static ProductAttributeOptionSetId Create(Guid value)
    {
        return new(value);
    }

    public static ProductAttributeOptionSetId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
