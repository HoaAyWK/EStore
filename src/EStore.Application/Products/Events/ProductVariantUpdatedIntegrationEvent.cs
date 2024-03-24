using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductVariantUpdatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; set; }

    public ProductVariantId ProductVariantId { get; set; }

    internal ProductVariantUpdatedIntegrationEvent(
        ProductVariantUpdatedDomainEvent productVariantUpdatedDomainEvent)
    {
        ProductId = productVariantUpdatedDomainEvent.ProductId;
        ProductVariantId = productVariantUpdatedDomainEvent.ProductVariantId;
    }

    [JsonConstructor]
    private ProductVariantUpdatedIntegrationEvent(
        ProductId productId,
        ProductVariantId productVariantId)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
    }
}
