using EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;
using EStore.Contracts.Notifications;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class NotificationsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public NotificationsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotificationsByCustomerId(
        [FromQuery] GetNotificationsByCustomerRequest request)
    {
        var query = _mapper.Map<GetNotificationsByCustomerRequest, GetNotificationsByCustomerIdQuery>(request);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
