using System.Text.Json.Serialization;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Events;

public class ProductReviewUpdatedIntegrationEvent : IIntegrationEvent
{
    public ProductId ProductId { get; }

    public ProductReviewId ProductReviewId { get; }

    public int OldRating { get; }

    public int NewRating { get; }

    internal ProductReviewUpdatedIntegrationEvent(
        ProductReviewUpdatedDomainEvent productReviewUpdatedDomainEvent)
    {
        ProductId = productReviewUpdatedDomainEvent.ProductId;
        ProductReviewId = productReviewUpdatedDomainEvent.ProductReviewId;
        OldRating = productReviewUpdatedDomainEvent.OldRating;
        NewRating = productReviewUpdatedDomainEvent.NewRating;
    }

    [JsonConstructor]
    private ProductReviewUpdatedIntegrationEvent(
        ProductId productId,
        ProductReviewId productReviewId,
        int oldRating,
        int newRating)
    {
        ProductId = productId;
        ProductReviewId = productReviewId;
        OldRating = oldRating;
        NewRating = newRating;
    }
}
