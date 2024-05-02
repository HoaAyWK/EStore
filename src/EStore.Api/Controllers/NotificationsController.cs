using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Notifications.Command.MarkAllNotificationsAsRead;
using EStore.Application.Notifications.Command.MarkNotificationAsRead;
using EStore.Application.Notifications.Queries.GetNotificationById;
using EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;
using EStore.Contracts.Notifications;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IWorkContext _workContext;

    public NotificationsController(
        ISender mediator,
        IMapper mapper,
        IWorkContext workContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _workContext = workContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotificationsByCustomerId(
        [FromQuery] GetNotificationsByCustomerRequest request)
    {
        var query = _mapper.Map<GetNotificationsByCustomerRequest, GetNotificationsByCustomerIdQuery>(request);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet(ApiRoutes.Notifications.GetMyNotifications)]
    public async Task<IActionResult> GetMyNotifications()
    {
        var customerId = _workContext.CustomerId;
        var query = _mapper.Map<Guid, GetNotificationsByCustomerIdQuery>(customerId);
        var getMyNotificationsResult = await _mediator.Send(query);

        return Ok(getMyNotificationsResult);
    }

    [HttpPut(ApiRoutes.Notifications.MarkAsRead)]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id)
    {
        var command = _mapper.Map<Guid, MarkNotificationAsReadCommand>(id);
        var markResult = await _mediator.Send(command);

        if (markResult.IsError)
        {
            return Problem(markResult.Errors);
        }

        var query = _mapper.Map<Guid, GetNotificationByIdQuery>(id);
        var getNotificationResult = await _mediator.Send(query);

        return getNotificationResult.Match(Ok, Problem);
    }

    [HttpPut(ApiRoutes.Notifications.MarkAllAsRead)]
    public async Task<IActionResult> MarkAllNotificationsAsRead()
    {
        var customerId = _workContext.CustomerId;
        var command = _mapper.Map<Guid, MarkAllNotificationsAsReadCommand>(customerId);

        var markResult = await _mediator.Send(command);

        if (markResult.IsError)
        {
            return Problem(markResult.Errors);
        }

        var query = _mapper.Map<Guid, GetNotificationsByCustomerIdQuery>(customerId);
        var getNotificationsResult = await _mediator.Send(query);

        return Ok(getNotificationsResult);
    }
}
