using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate;
using MediatR;

namespace EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;

public record GetNotificationsByCustomerIdQuery(
    CustomerId CustomerId)
    : IRequest<List<Notification>>;
