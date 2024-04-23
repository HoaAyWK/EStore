using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Orders.Commands.CreateOrder;
using EStore.Application.Orders.Commands.RefundOrder;
using EStore.Application.Orders.Queries.GetOrderById;
using EStore.Application.Orders.Queries.GetOrderListPaged;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.Enumerations;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetOrders(int page = 1, int pageSize = 5)
    {
        var query = _mapper.Map<GetOrderListPagedQuery>((page, pageSize));
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
            order => CreatedAtAction(nameof(GetOrder), new { id = order.Id }, _mapper.Map<OrderResponse>(order)),
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
}

