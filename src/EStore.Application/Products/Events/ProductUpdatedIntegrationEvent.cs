using System.Text.Json.Serialization;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Events;

public class ProductUpdatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public bool PreviousHasVariant { get; set; }

    internal ProductUpdatedIntegrationEvent(ProductUpdatedDomainEvent productUpdatedDomainEvent)
    {
        ProductId = productUpdatedDomainEvent.ProductId;
        PreviousHasVariant = productUpdatedDomainEvent.PreviousHasVariant;
    }

    [JsonConstructor]
    private ProductUpdatedIntegrationEvent(ProductId productId, bool previousHasVariant)
    {
        ProductId = productId;
        PreviousHasVariant = previousHasVariant;
    }
}
