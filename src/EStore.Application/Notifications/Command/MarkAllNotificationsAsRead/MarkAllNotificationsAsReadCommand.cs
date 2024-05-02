using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Notifications.Command.MarkAllNotificationsAsRead;

public record MarkAllNotificationsAsReadCommand(CustomerId CustomerId)
    : IRequest<ErrorOr<Updated>>;
