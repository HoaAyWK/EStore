using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate.ValueObjects;

public sealed class Rating : ValueObject
{
    public double Value { get; private set; }

    private Rating(double rating)
    {
        Value = rating;
    }

    public Rating Create(double rating)
    {
        return new(rating);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
