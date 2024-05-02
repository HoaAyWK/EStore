using EStore.Application.Notifications.Services;
using EStore.Contracts.Notifications;
using EStore.Domain.NotificationAggregate;
using EStore.Domain.NotificationAggregate.Repositories;
using MediatR;

namespace EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;

public class GetNotificationsByCustomerIdQueryHandler
    : IRequestHandler<GetNotificationsByCustomerIdQuery, List<NotificationResponse>>
{
    private readonly INotificationReadService _notificationReadService;

    public GetNotificationsByCustomerIdQueryHandler(
        INotificationReadService notificationReadService)
    {
        _notificationReadService = notificationReadService;
    }

    public async Task<List<NotificationResponse>> Handle(
        GetNotificationsByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _notificationReadService.GetNotificationsByCustomerId(request.CustomerId);
    }
}
