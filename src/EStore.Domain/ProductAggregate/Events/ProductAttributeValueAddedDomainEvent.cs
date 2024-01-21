using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public record ProductAttributeValueAddedDomainEvent(
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId)
    : IDomainEvent;
