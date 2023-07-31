namespace EStore.Domain.Common.Abstractions;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent);
}
