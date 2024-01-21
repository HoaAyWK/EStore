using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductAttributeValueRemovedDomainEventHandler
    : INotificationHandler<ProductAttributeValueRemovedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductAttributeValueRemovedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        ProductAttributeValueRemovedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new ProductAttributeValueRemovedIntegrationEvent(notification));
    }
}
