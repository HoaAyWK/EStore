using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate;
using EStore.Domain.NotificationAggregate.Repositories;
using EStore.Domain.NotificationAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly EStoreDbContext _dbContext;

    public NotificationRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Notification notification,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications.AddAsync(
            notification,
            cancellationToken);
    }

    public async Task<Notification?> GetByIdAsync(NotificationId id)
    {
        return await _dbContext.Notifications.FindAsync(id);
    }

    public async Task<List<Notification>> GetByOwnerIdAsync(CustomerId ownerId)
    {
        return await _dbContext.Notifications
            .Where(n => n.To == ownerId)
            .OrderByDescending(n => n.Timestamp)
            .ToListAsync();
    }

    public async Task UpdateNotificationsByCustomerId(
        CustomerId customerId,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications
            .Where(n => n.To == customerId)
            .ExecuteUpdateAsync(
                s => s.SetProperty(n => n.IsRead, true),
                cancellationToken: cancellationToken);
    }
}
