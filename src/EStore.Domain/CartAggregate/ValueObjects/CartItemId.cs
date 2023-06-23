using EStore.Domain.Common.Models;

namespace EStore.Domain.CartAggregate.ValueObjects;

public sealed class CartItemId : ValueObject
{
    public Guid Value { get; }

    private CartItemId(Guid value)
    {
        Value = value;
    }

    public static CartItemId Create(Guid value)
    {
        return new(value);
    }

    public static CartItemId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
