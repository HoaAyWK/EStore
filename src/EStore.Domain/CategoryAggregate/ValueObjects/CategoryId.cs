using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.CategoryAggregate.ValueObjects;

public sealed class CategoryId : ValueObject
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private CategoryId(string value)
    {
        Value = new Guid(value);
    }

    public static CategoryId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static CategoryId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
