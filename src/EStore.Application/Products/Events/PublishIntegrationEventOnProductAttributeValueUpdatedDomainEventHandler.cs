using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductAttributeValueUpdatedDomainEventHandler
    : INotificationHandler<ProductAttributeValueUpdatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductAttributeValueUpdatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ProductAttributeValueUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new ProductAttributeValueUpdatedIntegrationEvent(notification));
    }
}
