using ErrorOr;
using EStore.Contracts.Notifications;
using EStore.Domain.NotificationAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Notifications.Queries.GetNotificationById;

public record GetNotificationByIdQuery(NotificationId Id)
    : IRequest<ErrorOr<NotificationResponse>>;
