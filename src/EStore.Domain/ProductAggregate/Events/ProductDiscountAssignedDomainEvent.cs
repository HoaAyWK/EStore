using EStore.Domain.Common.Models;
using EStore.Domain.DiscountAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public record ProductDiscountAssignedDomainEvent(
    ProductId ProductId,
    DiscountId? DiscountId)
    : IDomainEvent;