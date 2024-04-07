using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Events;

public record ProductUpdatedDomainEvent(ProductId ProductId, bool PreviousHasVariant)
    : IDomainEvent;
