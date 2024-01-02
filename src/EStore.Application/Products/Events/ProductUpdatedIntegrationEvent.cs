using System.Text.Json.Serialization;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Events;

public class ProductUpdatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    internal ProductUpdatedIntegrationEvent(ProductUpdatedDomainEvent productUpdatedDomainEvent)
    {
        ProductId = productUpdatedDomainEvent.ProductId;
    }

    [JsonConstructor]
    private ProductUpdatedIntegrationEvent(ProductId productId)
    {
        ProductId = productId;
    }
}
