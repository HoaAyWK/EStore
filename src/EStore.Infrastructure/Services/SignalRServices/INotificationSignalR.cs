using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Infrastructure.Services.SignalRServices;

public interface INotificationSignalR
{
    public Task SendNotificationToCustomerOnOrderStatusChange(
        CustomerId customerId,
        NotificationResponse notification);
}
