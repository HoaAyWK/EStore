using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductAttributeValueCreatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }
    public ProductAttributeId ProductAttributeId { get; }
    public ProductAttributeValueId ProductAttributeValueId { get; }

    public ProductAttributeValueCreatedIntegrationEvent(
        ProductAttributeValueAddedDomainEvent domainEvent)
    {
        ProductId = domainEvent.ProductId;
        ProductAttributeId = domainEvent.ProductAttributeId;
        ProductAttributeValueId = domainEvent.ProductAttributeValueId;
    }

    [JsonConstructor]
    private ProductAttributeValueCreatedIntegrationEvent(
        ProductId productId,
        ProductAttributeId productAttributeId,
        ProductAttributeValueId productAttributeValueId)
    {
        ProductId = productId;
        ProductAttributeId = productAttributeId;
        ProductAttributeValueId = productAttributeValueId;
    }
}
