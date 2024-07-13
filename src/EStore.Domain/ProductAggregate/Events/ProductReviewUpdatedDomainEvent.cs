using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public record ProductReviewUpdatedDomainEvent(
    ProductId ProductId,
    ProductReviewId ProductReviewId,
    int OldRating,
    int NewRating)
    : IDomainEvent;
