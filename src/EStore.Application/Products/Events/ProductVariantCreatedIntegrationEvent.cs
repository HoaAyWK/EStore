using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductVariantCreatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; set; }

    public ProductVariantId ProductVariantId { get; set; }

    internal ProductVariantCreatedIntegrationEvent(
        ProductVariantCreatedDomainEvent productVariantCreatedDomainEvent)
    {
        ProductId = productVariantCreatedDomainEvent.ProductId;
        ProductVariantId = productVariantCreatedDomainEvent.ProductVariantId;
    }

    [JsonConstructor]
    private ProductVariantCreatedIntegrationEvent(
        ProductId productId,
        ProductVariantId productVariantId)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
    }
}
