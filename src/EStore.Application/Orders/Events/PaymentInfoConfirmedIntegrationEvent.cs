using System.Text.Json.Serialization;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Application.Orders.Events;

public class PaymentInfoConfirmedIntegrationEvent : IIntegrationEvent
{
    public OrderId OrderId { get; set; }

    internal PaymentInfoConfirmedIntegrationEvent(
        PaymentInfoConfirmedDomainEvent @event)
    {
        OrderId = @event.OrderId;
    }

    [JsonConstructor]
    private PaymentInfoConfirmedIntegrationEvent(OrderId orderId)
    {
        OrderId = orderId;
    }
}