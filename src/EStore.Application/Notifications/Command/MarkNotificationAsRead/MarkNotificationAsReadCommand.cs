using ErrorOr;
using EStore.Domain.NotificationAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Notifications.Command.MarkNotificationAsRead;

public record MarkNotificationAsReadCommand(NotificationId NotificationId)
    : IRequest<ErrorOr<Updated>>;
