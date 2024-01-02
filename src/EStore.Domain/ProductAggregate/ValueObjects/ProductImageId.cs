using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.ProductAggregate.ValueObjects;

public class ProductImageId : ValueObject
{
    public Guid Value { get; }
    
    public ProductImageId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private ProductImageId(string value)
    {
        Value = new Guid(value);
    }

    public static ProductImageId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ProductImageId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
