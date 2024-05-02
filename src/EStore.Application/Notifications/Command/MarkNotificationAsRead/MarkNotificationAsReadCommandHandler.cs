using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.NotificationAggregate.Repositories;
using MediatR;

namespace EStore.Application.Notifications.Command.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandHandler
    : IRequestHandler<MarkNotificationAsReadCommand, ErrorOr<Updated>>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);

        if (notification is null)
        {
            return Errors.Notification.NotFound;
        }

        notification.MarkAsRead();

        return Result.Updated;   
    }
}
