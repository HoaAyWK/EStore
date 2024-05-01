using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Queries.GetCustomerByEmail;
using EStore.Application.Customers.Queries.GetCustomerById;
using EStore.Application.Notifications.Command.CreateNotification;
using EStore.Application.Notifications.Queries.GetNotificationById;
using EStore.Application.Orders.Queries.GetOrderById;
using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Infrastructure.Persistence.Seeds;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace EStore.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationsHub : Hub<INotificationClient>
{
    public const string HubUrl = "/hubs/notifications";
    private readonly UserSeedingSettings _adminSettings;
    private readonly ISender _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public NotificationsHub(
        IOptions<UserSeedingSettings> adminSettingsOptions,
        ISender mediator,
        IDateTimeProvider dateTimeProvider)
    {
        _adminSettings = adminSettingsOptions.Value;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
    }

    public override async Task OnConnectedAsync()
    {
        var customerId = Context.User?.Identity?.Name;

        if (customerId != null)
        {
            await Clients.User(customerId).ReceiveNotification($"Welcome {customerId} to the store!");
        }
        else
        {
            await Clients.Caller.ReceiveNotification($"Welcome to the store! Please login to receive notifications.");
        }

        await base.OnConnectedAsync();
    }

    public async Task NotifyAdminWhenOrderPlaced(Guid orderId)
    {
        var customerId = Context.User?.Identity?.Name;

        if (customerId is null)
        {
            return;
        }

        var getCustomerQuery = new GetCustomerByIdQuery(
            CustomerId.Create(new Guid(customerId)));

        var getCustomerResult = await _mediator.Send(getCustomerQuery);

        if (getCustomerResult.IsError)
        {
            return;
        }

        var customer = getCustomerResult.Value;
        var getAdminQuery = new GetCustomerByEmailQuery(_adminSettings.Email);
        var getAdminResult = await _mediator.Send(getAdminQuery);

        if (getAdminResult.IsError)
        {
            return;
        }

        var admin = getAdminResult.Value;

        var command = new CreateNotificationCommand(
            Message: $"New order placed by {customer.FirstName} {customer.LastName}",
            Domain: nameof(Order),
            Type: OrderStatusHistory.OrderPlaced.Name,
            EntityId: orderId,
            From: CustomerId.Create(customer.Id),
            To: CustomerId.Create(admin.Id),
            IsRead: false,
            Timestamp: _dateTimeProvider.UtcNow);

        var createNotificationResult = await _mediator.Send(command);

        if (createNotificationResult.IsError)
        {
            return;
        }

        var query = new GetNotificationByIdQuery(createNotificationResult.Value.Id);
        var getNotificationResult = await _mediator.Send(query);

        if (getNotificationResult.IsError)
        {
            return;
        }

        await Clients.User(admin.Id.ToString())
            .NotifyAdminWhenOrderStatusChange(getNotificationResult.Value);
    }

    public async Task NotifyAdminWhenOrderCancelled(Guid orderId)
    {
        var customerId = Context.User?.Identity?.Name;

        if (customerId is null)
        {
            return;
        }

        var getCustomerQuery = new GetCustomerByIdQuery(
            CustomerId.Create(new Guid(customerId)));

        var getCustomerResult = await _mediator.Send(getCustomerQuery);

        if (getCustomerResult.IsError)
        {
            return;
        }

        var customer = getCustomerResult.Value;
        var getAdminQuery = new GetCustomerByEmailQuery(_adminSettings.Email);
        var getAdminResult = await _mediator.Send(getAdminQuery);

        if (getAdminResult.IsError)
        {
            return;
        }

        var admin = getAdminResult.Value;

        var command = new CreateNotificationCommand(
            Message: $"Order cancelled by {customer.FirstName} {customer.LastName}",
            Domain: nameof(Order),
            Type: OrderStatus.Cancelled.Name,
            EntityId: orderId,
            From: CustomerId.Create(customer.Id),
            To: CustomerId.Create(admin.Id),
            IsRead: false,
            Timestamp: _dateTimeProvider.UtcNow);

        var createNotificationResult = await _mediator.Send(command);

        if (createNotificationResult.IsError)
        {
            return;
        }

        var query = new GetNotificationByIdQuery(createNotificationResult.Value.Id);
        var getNotificationResult = await _mediator.Send(query);

        if (getNotificationResult.IsError)
        {
            return;
        }

        await Clients.User(admin.Id.ToString())
            .NotifyAdminWhenOrderStatusChange(getNotificationResult.Value);
    }

    public async Task NotifyAdminWhenOrderCompletedByCustomer(Guid orderId)
    {
        var customerId = Context.User?.Identity?.Name;

        if (customerId is null)
        {
            return;
        }

        var getCustomerQuery = new GetCustomerByIdQuery(
            CustomerId.Create(new Guid(customerId)));

        var getCustomerResult = await _mediator.Send(getCustomerQuery);

        if (getCustomerResult.IsError)
        {
            return;
        }

        var customer = getCustomerResult.Value;
        var getAdminQuery = new GetCustomerByEmailQuery(_adminSettings.Email);
        var getAdminResult = await _mediator.Send(getAdminQuery);

        if (getAdminResult.IsError)
        {
            return;
        }

        var admin = getAdminResult.Value;

        var command = new CreateNotificationCommand(
            Message: $"Order completed by {customer.FirstName} {customer.LastName}",
            Domain: nameof(Order),
            Type: OrderStatusHistory.OrderCompleted.Name,
            EntityId: orderId,
            From: CustomerId.Create(customer.Id),
            To: CustomerId.Create(admin.Id),
            IsRead: false,
            Timestamp: _dateTimeProvider.UtcNow);

        var createNotificationResult = await _mediator.Send(command);

        if (createNotificationResult.IsError)
        {
            return;
        }

        var query = new GetNotificationByIdQuery(createNotificationResult.Value.Id);
        var getNotificationResult = await _mediator.Send(query);

        if (getNotificationResult.IsError)
        {
            return;
        }

        await Clients.User(admin.Id.ToString())
            .NotifyAdminWhenOrderStatusChange(getNotificationResult.Value);
    }

    public async Task NotifyCustomerWhenPaymentInfoConfirmed(Guid orderId)
    {
        var getAdminQuery = new GetCustomerByEmailQuery(_adminSettings.Email);
        var getAdminResult = await _mediator.Send(getAdminQuery);

        if (getAdminResult.IsError)
        {
            return;
        }

        var adminId = getAdminResult.Value.Id;
        var getOrderQuery = new GetOrderByIdQuery(OrderId.Create(orderId));
        var getOrderResult = await _mediator.Send(getOrderQuery);

        if (getOrderResult.IsError)
        {
            return;
        }

        var order = getOrderResult.Value;
        var command = new CreateNotificationCommand(
            Message: $"Your order's payment information has been confirmed.",
            Domain: nameof(Order),
            Type: OrderStatusHistory.PaymentInfoConfirmed.Name,
            EntityId: orderId,
            From: CustomerId.Create(adminId),
            To: order.CustomerId,
            IsRead: false,
            Timestamp: _dateTimeProvider.UtcNow);

        var createNotificationResult = await _mediator.Send(command);

        if (createNotificationResult.IsError)
        {
            return;
        }

        var query = new GetNotificationByIdQuery(createNotificationResult.Value.Id);
        var getNotificationResult = await _mediator.Send(query);

        if (getNotificationResult.IsError)
        {
            return;
        }

        await Clients.User(order.CustomerId.Value.ToString())
            .NotifyCustomerWhenOrderStatusChange(getNotificationResult.Value);
    }
}

public interface INotificationClient
{
    Task ReceiveNotification(string notification);
    Task NotifyAdminWhenOrderStatusChange(NotificationResponse notification);
    Task NotifyCustomerWhenOrderStatusChange(NotificationResponse notification);
}
