using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Orders.Commands.ConfirmPaymentInfo;
using EStore.Application.Orders.Commands.ConfirmReceived;
using EStore.Application.Orders.Commands.CreateOrder;
using EStore.Application.Orders.Commands.RefundOrder;
using EStore.Application.Orders.Queries.GetOrderById;
using EStore.Application.Orders.Queries.GetOrderListPaged;
using EStore.Application.Orders.Queries.GetOrdersByCustomer;
using EStore.Application.Orders.Queries.GetOrderStatuses;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OrdersController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IWorkContext _workContext;

    public OrdersController(
        ISender mediator,
        IMapper mapper,
        IWorkContext workContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _workContext = workContext;
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Order.GetStatuses)]
    public async Task<IActionResult> GetOrderStatuses()
    {
        var query = new GetOrderStatusesQuery();
        var orderStatuses = await _mediator.Send(query);

        return Ok(orderStatuses);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] GetOrdersRequest request)
    {
        var query = _mapper.Map<GetOrdersRequest, GetOrderListPagedQuery>(request);

        return Ok(await _mediator.Send(query));
    }

    [HttpGet(ApiRoutes.Order.GetMyOrders)]
    public async Task<IActionResult> GetMyOrders([FromQuery] GetMyOrdersRequest request)
    {
        var query = _mapper.Map<(Guid, GetMyOrdersRequest), GetOrdersByCustomerQuery>((
            _workContext.CustomerId, request));

        var listPaged = await _mediator.Send(query);

        return Ok(_mapper.Map<PagedList<OrderResponse>>(listPaged));
    }

    [HttpGet(ApiRoutes.Order.Get)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var query = _mapper.Map<GetOrderByIdQuery>(id);
        var getOderResult = await _mediator.Send(query);

        return getOderResult.Match(
            order => Ok(_mapper.Map<OrderResponse>(order)),
            Problem);
    }

    [HttpPost(ApiRoutes.Order.Create)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var customerId = _workContext.CustomerId;
        var command = _mapper.Map<(Guid, CreateOrderRequest), CreateOrderCommand>((customerId, request));

        var createOrderResult = await _mediator.Send(command);

        return createOrderResult.Match(
            order => CreatedAtAction(
                nameof(GetOrder),
                new { id = order.Id.Value },
                _mapper.Map<OrderResponse>(order)),
            Problem);
    }

    [HttpPost(ApiRoutes.Order.Refund)]
    public async Task<IActionResult> RefundOrder([FromRoute] Guid id)
    {
        var command = _mapper.Map<RefundOrderCommand>(id);
        var refundOrderResult = await _mediator.Send(command);

        return refundOrderResult.Match(
            success => NoContent(),
            Problem);
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPut(ApiRoutes.Order.ConfirmPaymentInfo)]
    public async Task<IActionResult> ConfirmPaymentInfo([FromRoute] Guid id)
    {
        var command = _mapper.Map<Guid, ConfirmPaymentInfoCommand>(id);
        var confirmPaymentInfoResult = await _mediator.Send(command);
        var query = _mapper.Map<Guid, GetOrderByIdQuery>(id);
        var getOrderResult = await _mediator.Send(query);

        return getOrderResult.Match(Ok, Problem);
    }

    [HttpPut(ApiRoutes.Order.ConfirmReceived)]
    public async Task<IActionResult> ConfirmReceived([FromRoute] Guid id)
    {
        var command = _mapper.Map<Guid, ConfirmReceivedCommand>(id);
        var confirmReceivedResult = await _mediator.Send(command);

        if (confirmReceivedResult.IsError)
        {
            return Problem(confirmReceivedResult.Errors);
        }

        var query = _mapper.Map<Guid, GetOrderByIdQuery>(id);
        var getOrderResult = await _mediator.Send(query);

        return getOrderResult.Match(Ok, Problem);
    }
}

