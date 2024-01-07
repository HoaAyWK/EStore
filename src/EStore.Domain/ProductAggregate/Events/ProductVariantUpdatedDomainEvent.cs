using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public class ProductVariantUpdatedDomainEvent : IDomainEvent
{
    public ProductVariantUpdatedDomainEvent(
        ProductId productId,
        ProductVariantId productVariantId)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
    }

    public ProductId ProductId { get; }

    public ProductVariantId ProductVariantId { get; }
}
