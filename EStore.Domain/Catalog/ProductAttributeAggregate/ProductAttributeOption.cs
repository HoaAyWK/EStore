using EStore.Domain.Common.Models;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAttributeAggregate;

public sealed class ProductAttributeOption : Entity<ProductAttributeOptionId>
{
    public string Name { get; private set; } = null!;

    public string? Alias { get; private set; }

    public decimal PriceAdjustment { get; private set; }

    private ProductAttributeOption(
        ProductAttributeOptionId id,
        string name,
        decimal priceAdjustment,
        string? alias
        ) : base(id)
    {
        Name = name;
        PriceAdjustment = priceAdjustment;
        Alias = alias;
    }

    public static ProductAttributeOption Create(
        string name,
        decimal priceAdjustment,
        string? alias = null)
    {
        return new(
            ProductAttributeOptionId.CreateUnique(),
            name,
            priceAdjustment,
            alias);
    }
}
