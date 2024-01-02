using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductCreatedDomainEventHandler
    : INotificationHandler<ProductCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductCreatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        ProductCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new ProductCreatedIntegrationEvent(notification));
    }
}
