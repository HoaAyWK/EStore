using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductUpdatedDomainEventHandler
    : INotificationHandler<ProductUpdatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductUpdatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        ProductUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new ProductUpdatedIntegrationEvent(notification));
    }
}
