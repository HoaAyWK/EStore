using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate.Events;

public record OrderCreatedDomainEvent(OrderId OrderId, CustomerId CustomerId) : IDomainEvent;
