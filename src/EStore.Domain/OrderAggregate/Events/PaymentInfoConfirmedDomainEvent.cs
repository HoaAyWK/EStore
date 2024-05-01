using EStore.Domain.Common.Models;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate.Events;

public record PaymentInfoConfirmedDomainEvent(
    OrderId OrderId)
    : IDomainEvent;
