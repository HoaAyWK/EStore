using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.BrandAggregate;

public sealed class Brand : AggregateRoot<BrandId>
{
    public string Name { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private Brand()
    {
    }

    private Brand(
        BrandId brandId,
        string name,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(brandId)
    {
        Name = name;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Brand Create(string name)
    {
        return new(
            BrandId.CreateUnique(),
            name,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}
