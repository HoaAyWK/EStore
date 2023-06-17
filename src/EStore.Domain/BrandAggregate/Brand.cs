using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.BrandAggregate;

public sealed class Brand : AggregateRoot<BrandId>, IAuditableEntity
{
    public string Name { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private Brand()
    {
    }

    private Brand(
        BrandId brandId,
        string name)
        : base(brandId)
    {
        Name = name;
    }

    public static Brand Create(string name)
    {
        return new(
            BrandId.CreateUnique(),
            name);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
