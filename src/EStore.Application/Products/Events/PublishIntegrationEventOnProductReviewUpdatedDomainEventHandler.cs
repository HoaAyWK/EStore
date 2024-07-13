using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductReviewUpdatedDomainEventHandler
    : INotificationHandler<ProductReviewUpdatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductReviewUpdatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        ProductReviewUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
        => await _integrationEventPublisher.PublishAsync(
            new ProductReviewUpdatedIntegrationEvent(notification));
}