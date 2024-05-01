using ErrorOr;
using EStore.Application.Notifications.Services;
using EStore.Contracts.Notifications;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Notifications.Queries.GetNotificationById;

public class GetNotificationByIdQueryHandler
    : IRequestHandler<GetNotificationByIdQuery, ErrorOr<NotificationResponse>>
{
    private readonly INotificationReadService _notificationReadService;

    public GetNotificationByIdQueryHandler(
        INotificationReadService notificationReadService)
    {
        _notificationReadService = notificationReadService;
    }

    public async Task<ErrorOr<NotificationResponse>> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var notification = await _notificationReadService.GetNotificationById(request.Id);

        if (notification is null)
        {
            return Errors.Notification.NotFound;
        }

        return notification;
    }
}
