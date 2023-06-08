using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public sealed class ProductAttributeSelectionId : ValueObject
{
    public Guid Value { get; }

    private ProductAttributeSelectionId(Guid value)
    {
        Value = value;
    }

    public static ProductAttributeSelectionId Create(Guid value)
    {
        return new(value);
    }

    public static ProductAttributeSelectionId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
