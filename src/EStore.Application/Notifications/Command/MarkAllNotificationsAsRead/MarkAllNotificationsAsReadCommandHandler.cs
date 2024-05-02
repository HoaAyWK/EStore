using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.NotificationAggregate.Repositories;
using MediatR;

namespace EStore.Application.Notifications.Command.MarkAllNotificationsAsRead;

public class MarkAllNotificationsAsReadCommandHandler
    : IRequestHandler<MarkAllNotificationsAsReadCommand, ErrorOr<Updated>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICustomerRepository _customerRepository;

    public MarkAllNotificationsAsReadCommandHandler(
        INotificationRepository notificationRepository,
        ICustomerRepository customerRepository)
    {
        _notificationRepository = notificationRepository;
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        MarkAllNotificationsAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        await _notificationRepository.UpdateNotificationsByCustomerId(
            request.CustomerId,
            cancellationToken);

        return Result.Updated;
    }
}
