using System.Text.Json.Serialization;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.DiscountAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Events;

public class ProductDiscountAssignedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public DiscountId? DiscountId { get; }

    internal ProductDiscountAssignedIntegrationEvent(
        ProductDiscountAssignedDomainEvent productDiscountAssignedDomainEvent)
    {
        ProductId = productDiscountAssignedDomainEvent.ProductId;
        DiscountId = productDiscountAssignedDomainEvent.DiscountId;
    }

    [JsonConstructor]
    private ProductDiscountAssignedIntegrationEvent(
        ProductId productId,
        DiscountId? discountId)
    {
        ProductId = productId;
        DiscountId = discountId;
    }
}
