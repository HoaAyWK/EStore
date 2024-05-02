using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;

public record GetNotificationsByCustomerIdQuery(
    CustomerId CustomerId)
    : IRequest<List<NotificationResponse>>;
