using EStore.Application.Orders.Events;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.OrderAggregate.Events;
using MediatR;

namespace EStore.Application.Invoices.Events;

public class PublishIntegrationEventOnPaymentInfoConfirmedDomainEventHandler
    : INotificationHandler<PaymentInfoConfirmedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnPaymentInfoConfirmedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        PaymentInfoConfirmedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new PaymentInfoConfirmedIntegrationEvent(notification));
    }
}
