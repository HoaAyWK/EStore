using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public record ProductAttributeValueUpdatedDomainEvent(
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId,
    decimal OldPrice,
    decimal NewPrice)
    : IDomainEvent;
