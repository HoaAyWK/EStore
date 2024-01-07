using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductVariantCreatedDomainEventHandler
    : INotificationHandler<ProductVariantCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductVariantCreatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        ProductVariantCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(
            new ProductVariantCreatedIntegrationEvent(notification));
    }
}
