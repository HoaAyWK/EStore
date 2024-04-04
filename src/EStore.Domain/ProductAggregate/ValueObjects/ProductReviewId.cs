using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public class ProductReviewId : ValueObject
{
    public Guid Value { get; }

    private ProductReviewId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private ProductReviewId(string value)
    {
        Value = new Guid(value);
    }

    public static ProductReviewId CreateUnique
        => new(Guid.NewGuid());

    public static ProductReviewId Create(Guid value)
        => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }
}
