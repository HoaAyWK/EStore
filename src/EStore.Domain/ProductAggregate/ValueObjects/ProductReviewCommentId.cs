using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public class ProductReviewCommentId : ValueObject
{
    public Guid Value { get; }

    private ProductReviewCommentId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private ProductReviewCommentId(string value)
    {
        Value = new Guid(value);
    }

    public static ProductReviewCommentId CreateUnique
        => new(Guid.NewGuid());

    public static ProductReviewCommentId Create(Guid value)
        => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }
}
