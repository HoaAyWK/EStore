using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductAttributeValueRemovedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public ProductAttributeId ProductAttributeId { get; }

    public ProductAttributeValueId ProductAttributeValueId { get; }

    internal ProductAttributeValueRemovedIntegrationEvent(
        ProductAttributeValueRemovedDomainEvent domainEvent)
    {
        ProductId = domainEvent.ProductId;
        ProductAttributeId = domainEvent.ProductAttributeId;
        ProductAttributeValueId = domainEvent.ProductAttributeValueId;
    }

    [JsonConstructor]
    private ProductAttributeValueRemovedIntegrationEvent(
        ProductId productId,
        ProductAttributeId productAttributeId,
        ProductAttributeValueId productAttributeValueId)
    {
        ProductId = productId;
        ProductAttributeId = productAttributeId;
        ProductAttributeValueId = productAttributeValueId;
    }
}
