using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate;
using MediatR;

namespace EStore.Application.Notifications.Command.CreateNotification;

public record CreateNotificationCommand(
    string Message,
    string Domain,
    string Type,
    Guid EntityId,
    CustomerId From,
    CustomerId To,
    bool IsRead,
    DateTime Timestamp)
    : IRequest<ErrorOr<Notification>>;
