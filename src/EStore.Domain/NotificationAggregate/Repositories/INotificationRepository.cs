using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Domain.NotificationAggregate.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetByOwnerIdAsync(CustomerId ownerId);

    Task AddAsync(
        Notification notification,
        CancellationToken cancellationToken = default);
}
