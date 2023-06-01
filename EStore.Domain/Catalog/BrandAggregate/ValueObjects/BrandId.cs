using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.BrandAggregate.ValueObjects;

public sealed class BrandId : ValueObject
{
    public Guid Value { get; }

    private BrandId(Guid value)
    {
        Value = value;
    }

    public static BrandId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static BrandId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
