using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.NotificationAggregate;
using EStore.Domain.NotificationAggregate.Repositories;
using MediatR;

namespace EStore.Application.Notifications.Command.CreateNotification;

public class CreateNotificationCommandHandler
    : IRequestHandler<CreateNotificationCommand, ErrorOr<Notification>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Notification>> Handle(
        CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = Notification.Create(
            request.Message,
            request.Domain,
            request.Type,
            request.EntityId,
            request.From,
            request.To,
            request.IsRead,
            _dateTimeProvider.UtcNow);

        await _notificationRepository.AddAsync(notification, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return notification;
    }
}
