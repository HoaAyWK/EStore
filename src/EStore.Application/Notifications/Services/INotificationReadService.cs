using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate.ValueObjects;

namespace EStore.Application.Notifications.Services;

public interface INotificationReadService
{
    Task<List<NotificationResponse>> GetNotificationsByCustomerId(CustomerId customerId);
        
    Task<NotificationResponse?> GetNotificationById(NotificationId notificationId);
}
