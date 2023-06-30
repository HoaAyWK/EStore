using EStore.Domain.Common.Models;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.ProductVariantAggregate.Events;

public sealed record ProductVariantCreatedDomainEvent(ProductVariantId ProductVariantId) : IDomainEvent;
