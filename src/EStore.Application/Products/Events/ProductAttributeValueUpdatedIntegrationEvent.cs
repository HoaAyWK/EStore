using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductAttributeValueUpdatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public ProductAttributeId ProductAttributeId { get; }

    public ProductAttributeValueId ProductAttributeValueId { get; }

    internal ProductAttributeValueUpdatedIntegrationEvent(
        ProductAttributeValueUpdatedDomainEvent domainEvent)
    {
        ProductId = domainEvent.ProductId;
        ProductAttributeId = domainEvent.ProductAttributeId;
        ProductAttributeValueId = domainEvent.ProductAttributeValueId;
    }

    [JsonConstructor]
    private ProductAttributeValueUpdatedIntegrationEvent(
        ProductId productId,
        ProductAttributeId productAttributeId,
        ProductAttributeValueId productAttributeValueId)
    {
        ProductId = productId;
        ProductAttributeId = productAttributeId;
        ProductAttributeValueId = productAttributeValueId;
    }
}
