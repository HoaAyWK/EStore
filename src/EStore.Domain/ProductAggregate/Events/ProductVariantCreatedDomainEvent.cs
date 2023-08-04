using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public sealed record ProductVariantCreatedDomainEvent(
    ProductId ProductId,
    ProductVariantId ProductVariantId)
    : IDomainEvent;
