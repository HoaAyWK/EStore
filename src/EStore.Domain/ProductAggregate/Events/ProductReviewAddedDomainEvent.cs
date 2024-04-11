using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public record ProductReviewAddedDomainEvent(
    ProductId ProductId,
    ProductVariantId? ProductVariantId,
    ProductReviewId ProductReviewId) : IDomainEvent;
