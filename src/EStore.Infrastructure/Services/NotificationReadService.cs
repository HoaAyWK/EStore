using EStore.Application.Notifications.Services;
using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate.ValueObjects;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

public class NotificationReadService : INotificationReadService
{
    private readonly EStoreDbContext _dbContext;

    public NotificationReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<NotificationResponse?> GetNotificationById(NotificationId notificationId)
    {
        var notificationWithFrom =
            from notification in _dbContext.Notifications.AsNoTracking()
            join customer in _dbContext.Customers.AsNoTracking()
            on notification.From equals customer.Id into ncs
            from nc in ncs.DefaultIfEmpty()
            where notification.Id == notificationId
            orderby notification.Timestamp descending
            select new NotificationResponse
            {
                Id = notification.Id.Value,
                Message = notification.Message,
                Domain = notification.Domain,
                Type = notification.Type,
                EntityId = notification.EntityId,
                From = new FromResponse(
                    nc.Id.Value,
                    $"{nc!.FirstName} {nc.LastName}",
                    nc.Email,
                    nc.AvatarUrl),
                To = notification.To.Value,
                IsRead = notification.IsRead,
                Timestamp = notification.Timestamp
            };

        return notificationWithFrom.SingleOrDefaultAsync();
    }

    public Task<List<NotificationResponse>> GetNotificationsByCustomerId(CustomerId customerId)
    {
        var notificationsWithFrom =
            from notification in _dbContext.Notifications.AsNoTracking()
            join customer in _dbContext.Customers.AsNoTracking()
            on notification.From equals customer.Id into ncs
            from nc in ncs.DefaultIfEmpty()
            where notification.To == customerId
            orderby notification.Timestamp descending
            select new NotificationResponse
            {
                Id = notification.Id.Value,
                Message = notification.Message,
                Domain = notification.Domain,
                Type = notification.Type,
                EntityId = notification.EntityId,
                From = new FromResponse(
                    customerId.Value,
                    $"{nc!.FirstName} {nc.LastName}",
                    nc.Email,
                    nc.AvatarUrl),
                To = notification.To.Value,
                IsRead = notification.IsRead,
                Timestamp = notification.Timestamp
            };

        return notificationsWithFrom.ToListAsync();
    }
}
