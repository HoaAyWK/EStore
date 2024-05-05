using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.BannerAggregate.ValueObjects;

public sealed class BannerId : ValueObject
{
    public Guid Value { get; }

    private BannerId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private BannerId(string value)
    {
        Value = new Guid(value);
    }

    public static BannerId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static BannerId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
