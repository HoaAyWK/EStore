using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductCreatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; private set; }

    internal ProductCreatedIntegrationEvent(
        ProductCreatedDomainEvent productCreatedDomainEvent)
    {
        ProductId = productCreatedDomainEvent.ProductId;
    }

    [JsonConstructor]
    private ProductCreatedIntegrationEvent(ProductId productId)
    {
        ProductId = productId;
    }
}
