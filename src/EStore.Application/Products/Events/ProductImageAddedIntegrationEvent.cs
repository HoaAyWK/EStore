using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Events;

public class ProductImageAddedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public ProductImageId ProductImageId { get; }

    internal ProductImageAddedIntegrationEvent(
        ProductImageAddedDomainEvent productImageAddedDomainEvent)
    {
        ProductId = productImageAddedDomainEvent.ProductId;
        ProductImageId = productImageAddedDomainEvent.ProductImageId;
    }

    [JsonConstructor]
    private ProductImageAddedIntegrationEvent(
        ProductId productId,
        ProductImageId productImageId)
    {
        ProductId = productId;
        ProductImageId = productImageId;
    }
}
