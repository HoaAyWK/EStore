using EStore.Domain.NotificationAggregate;
using EStore.Domain.NotificationAggregate.Repositories;
using MediatR;

namespace EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;

public class GetNotificationsByCustomerIdQueryHandler
    : IRequestHandler<GetNotificationsByCustomerIdQuery, List<Notification>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsByCustomerIdQueryHandler(
        INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<List<Notification>> Handle(
        GetNotificationsByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _notificationRepository.GetByOwnerIdAsync(request.CustomerId);
    }
}
