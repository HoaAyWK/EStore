using EStore.Domain.Common.Models;

namespace EStore.Domain.CategoryAggregate.ValueObjects;

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

    public static CategoryId? Create(Guid? value)
    {
        return value.HasValue ? new(value.Value) : null;
    }


    public static bool operator==(CategoryId? a, CategoryId? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator!=(CategoryId? a, CategoryId? b)
    {
        
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
