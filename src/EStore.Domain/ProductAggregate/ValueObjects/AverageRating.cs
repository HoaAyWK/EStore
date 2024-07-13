using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public sealed class AverageRating : ValueObject
{
    public double Value { get; private set; }

    public int NumRatings { get; private set; }

    private AverageRating(double value, int numRatings)
    {
        Value = value;
        NumRatings = numRatings;
    }

    private AverageRating()
    {
    }

    public static AverageRating Create(double rating = 0, int numRatings = 0)
    {
        return new(rating, numRatings);
    }

    public void AddNewRating(Rating rating)
    {
        Value = ((Value * NumRatings) + rating.Value) / ++NumRatings;
    }

    public void RemoveRating(Rating rating)
    {
        Value = ((Value * NumRatings) - rating.Value) / --NumRatings;
    }

    public void ResetRating()
    {
        Value = 0;
        NumRatings = 0;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
