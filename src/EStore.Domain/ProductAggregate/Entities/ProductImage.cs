using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Entities;

public class ProductImage : Entity<ProductImageId>
{
    public string ImageUrl { get; private set; } = null!;

    public bool IsMain { get; private set; }

    public int DisplayOrder { get; private set; }

    private ProductImage(
        ProductImageId id,
        string imageUrl,
        bool isMain,
        int displayOrder)
        : base(id)
    {
        ImageUrl = imageUrl;
        IsMain = isMain;
        DisplayOrder = displayOrder;
    }

    private ProductImage()
    {
    }

    public static ProductImage Create(
        string imageUrl,
        bool isMain,
        int displayOrder)
    {
        return new(
            ProductImageId.CreateUnique(),
            imageUrl,
            isMain,
            displayOrder);
    }

    public void SetNormal()
    {
        IsMain = false;
    }
}
