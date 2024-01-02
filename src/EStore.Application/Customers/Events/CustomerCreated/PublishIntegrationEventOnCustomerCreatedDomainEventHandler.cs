using EStore.Application.Customers.Events.CustomerCreated;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.CustomerAggregate.Events;
using MediatR;

namespace EStore.Application.Customers.Events;

public class PublishIntegrationEventOnCustomerCreatedDomainEventHandler
    : INotificationHandler<CustomerCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnCustomerCreatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        CustomerCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new CustomerCreatedIntegrationEvent(notification));
    }
}
