using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.CategoryAggregate.ValueObjects;

public sealed class CategoryId : ValueObject
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static CategoryId Create(Guid value)
    {
        return new(value);
    }
}
