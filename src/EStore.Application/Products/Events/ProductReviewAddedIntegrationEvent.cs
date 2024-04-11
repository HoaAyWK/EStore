using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductReviewAddedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public ProductVariantId? ProductVariantId { get; }

    public ProductReviewId ProductReviewId { get; }

    internal ProductReviewAddedIntegrationEvent(
        ProductReviewAddedDomainEvent productReviewAddedDomainEvent)
    {
        ProductId = productReviewAddedDomainEvent.ProductId;
        ProductVariantId = productReviewAddedDomainEvent.ProductVariantId;
        ProductReviewId = productReviewAddedDomainEvent.ProductReviewId;
    }

    [JsonConstructor]
    private ProductReviewAddedIntegrationEvent(
        ProductId productId,
        ProductVariantId? productVariantId,
        ProductReviewId productReviewId)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
        ProductReviewId = productReviewId;
    }
}
