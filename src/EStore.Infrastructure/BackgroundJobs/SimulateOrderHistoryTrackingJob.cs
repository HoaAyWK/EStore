using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Queries.GetCustomerByEmail;
using EStore.Application.Notifications.Command.CreateNotification;
using EStore.Application.Notifications.Queries.GetNotificationById;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Infrastructure.Persistence;
using EStore.Infrastructure.Persistence.Seeds;
using EStore.Infrastructure.Services.SignalRServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

namespace EStore.Infrastructure.BackgroundJobs;

public class SimulateOrderHistoryTrackingJob : IJob
{
    private readonly EStoreDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INotificationSignalR _notificationSignalR;
    private readonly ISender _mediator;
    private readonly UserSeedingSettings _adminSettings;

    public SimulateOrderHistoryTrackingJob(
        EStoreDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        INotificationSignalR notificationSignalR,
        ISender mediator,
        IOptions<UserSeedingSettings> adminSettingsOptions)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _notificationSignalR = notificationSignalR;
        _mediator = mediator;
        _adminSettings = adminSettingsOptions.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var getAdminQuery = new GetCustomerByEmailQuery(_adminSettings.Email);
        var getAdminResult = await _mediator.Send(getAdminQuery, context.CancellationToken);

        if (getAdminResult.IsError)
        {
            // TODO: log error
            return;
        }

        var admin = getAdminResult.Value;

        var processingOrders = await _dbContext.Orders
            .Where(o => o.OrderStatus == OrderStatus.Processing) 
            .OrderByDescending(o => o.CreatedDateTime)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var order in processingOrders)
        {
            var lastOrderStatusHistory = order.OrderStatusHistoryTrackings
                .OrderByDescending(h => h.CreatedDateTime)
                .FirstOrDefault();

            if (lastOrderStatusHistory!.Status.Value >= OrderStatusHistory.PaymentInfoConfirmed.Value &&
                lastOrderStatusHistory.Status.Value < OrderStatusHistory.OrderReceived.Value)
            {
                var nextStatus = OrderStatusHistory.FromValue(lastOrderStatusHistory.Status.Value + 1)
                    ?? OrderStatusHistory.OrderReceived;
                    
                order.AddOrderStatusHistoryTracking(nextStatus, _dateTimeProvider.UtcNow);

                var result = await _dbContext.SaveChangesAsync(context.CancellationToken);

                if (result is 0)
                {
                    continue;
                }

                var message = "Your order has been shipped out.";

                if (nextStatus == OrderStatusHistory.InTransit)
                {
                    message = "Your order is in transit.";
                }
                else if (nextStatus == OrderStatusHistory.OrderReceived)
                {
                    message = "Your order has been received.";
                }

                var createNotificationCommand = new CreateNotificationCommand(
                    Message: message,
                    Domain: nameof(Order),
                    Type: nextStatus.Name,
                    EntityId: order.Id.Value,
                    From: CustomerId.Create(admin.Id),
                    To: order.CustomerId,
                    IsRead: false,
                    _dateTimeProvider.UtcNow);

                var createNotificationResult = await _mediator.Send(
                    createNotificationCommand,
                    context.CancellationToken);

                if (createNotificationResult.IsError)
                {
                    // TODO: log error

                    return;
                }

                var query = new GetNotificationByIdQuery(createNotificationResult.Value.Id);
                var getNotificationResult = await _mediator.Send(query);

                if (getNotificationResult.IsError)
                {
                    return;
                }

                await _notificationSignalR.SendNotificationToCustomerOnOrderStatusChange(
                    order.CustomerId,
                    getNotificationResult.Value);
            }
        }

    }
}
