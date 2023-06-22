using EStore.Domain.Common.Models;

namespace EStore.Domain.CartAggregate.ValueObjects;

public sealed class CartId : ValueObject
{
    public Guid Value { get; }

    private CartId(Guid value)
    {
        Value = value;
    }

    public static CartId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static CartId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
