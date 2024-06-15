using EStore.Domain.Common.Abstractions;
using EStore.Domain.ProductAggregate.Events;
using MediatR;

namespace EStore.Application.Products.Events;

public class PublishIntegrationEventOnProductDiscountAssignedDomainEventHandler
    : INotificationHandler<ProductDiscountAssignedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnProductDiscountAssignedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(
        ProductDiscountAssignedDomainEvent notification,
        CancellationToken cancellationToken)
        => await _integrationEventPublisher.PublishAsync(
            new ProductDiscountAssignedIntegrationEvent(notification));
}
