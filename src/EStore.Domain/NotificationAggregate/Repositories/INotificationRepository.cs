using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate.ValueObjects;

namespace EStore.Domain.NotificationAggregate.Repositories;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(NotificationId id);

    Task<List<Notification>> GetByOwnerIdAsync(CustomerId ownerId);

    Task AddAsync(
        Notification notification,
        CancellationToken cancellationToken = default);

    Task UpdateNotificationsByCustomerId(
        CustomerId customerId,
        CancellationToken cancellationToken = default);
}
