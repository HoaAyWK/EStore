using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Infrastructure.Services.SignalRServices;
using Microsoft.AspNetCore.SignalR;

namespace EStore.Api.SignalRServices;

public class NotificationSignalR : INotificationSignalR
{
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationSignalR(IHubContext<NotificationsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationToCustomerOnOrderStatusChange(
        CustomerId customerId,
        NotificationResponse notification)
    {
        await _hubContext.Clients.User(customerId.Value.ToString())
            .SendAsync(
                nameof(INotificationClient.NotifyCustomerWhenOrderStatusChange),
                notification);
    }
}
