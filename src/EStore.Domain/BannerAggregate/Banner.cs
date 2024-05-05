using EStore.Domain.BannerAggregate.Enumerations;
using EStore.Domain.BannerAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.BannerAggregate;

public sealed class Banner : AggregateRoot<BannerId>, IAuditableEntity, ISoftDeletableEntity
{
    public ProductId ProductId { get; private set; } = null!;

    public ProductVariantId? ProductVariantId { get; private set; }

    public BannerDirection Direction { get; private set; } = BannerDirection.Horizontal;

    public int DisplayOrder { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    private Banner()
    {
    }
    
    private Banner(
        BannerId id,
        ProductId productId,
        ProductVariantId? productVariantId,
        BannerDirection direction,
        int displayOrder,
        bool isActive)
        : base(id)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
        Direction = direction;
        DisplayOrder = displayOrder;
        IsActive = isActive;
    }

    public static Banner Create(
        ProductId productId,
        ProductVariantId? productVariantId,
        BannerDirection direction,
        int displayOrder,
        bool isActive)
    {
        return new Banner(
            BannerId.CreateUnique(),
            productId,
            productVariantId,
            direction,
            displayOrder,
            isActive);
    }

    public void UpdateDetails(
        BannerDirection direction,
        int displayOrder,
        bool isActive)
    {
        Direction = direction;
        DisplayOrder = displayOrder;
        IsActive = isActive;
    }
}
