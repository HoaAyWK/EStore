using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Domain.CustomerAggregate.Events;

public record CustomerCreatedDomainEvent(CustomerId CustomerId, string Email) : IDomainEvent;
