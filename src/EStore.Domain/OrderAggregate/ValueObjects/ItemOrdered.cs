using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate.ValueObjects;

public sealed class ItemOrdered : ValueObject
{
    public ProductId ProductId { get; private set; } = null!;

    public ProductVariantId? ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = null!;

    public string? ProductImage { get; private set; }

    public string? ProductAttributes { get; private set; }

    private ItemOrdered()
    {
    }

    private ItemOrdered(
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string productImage,
        string? productAttributes)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
        ProductName = productName;
        ProductImage = productImage;
        ProductAttributes = productAttributes;
    }

    public static ItemOrdered Create(
        ProductId productId,
        ProductVariantId? productVariantId,
        string productName,
        string productImage,
        string? productAttributes)
    {
        return new(
            productId,
            productVariantId,
            productName,
            productImage,
            productAttributes);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return ProductVariantId!;
    }
}
