using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate;
using EStore.Domain.NotificationAggregate.Repositories;
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

    public async Task<List<Notification>> GetByOwnerIdAsync(CustomerId ownerId)
    {
        return await _dbContext.Notifications
            .Where(n => n.To == ownerId)
            .OrderByDescending(n => n.Timestamp)
            .ToListAsync();
    }
}
