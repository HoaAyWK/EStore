using EStore.Domain.Common.Models;

namespace EStore.Domain.CartAggregate.ValueObjects;

public sealed class CartItemId : ValueObject
{
    public int Value { get; }

    private CartItemId(int value)
    {
        Value = value;
    }

    public static CartItemId Create(int value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
