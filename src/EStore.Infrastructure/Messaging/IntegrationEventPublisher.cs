using EStore.Domain.Common.Abstractions;
using MediatR;

namespace EStore.Infrastructure.Messaging;

public sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublisher _publisher;

    public IntegrationEventPublisher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishAsync(IIntegrationEvent integrationEvent)
    {
        await _publisher.Publish(integrationEvent);
    }
}
