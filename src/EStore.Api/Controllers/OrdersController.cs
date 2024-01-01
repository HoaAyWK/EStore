using EStore.Api.Common.ApiRoutes;
using EStore.Application.Orders.Commands.RefundOrder;
using EStore.Application.Orders.Queries.GetOrderById;
using EStore.Application.Orders.Queries.GetOrderListPaged;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class OrdersController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public OrdersController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
            errors => Problem(errors));
    }

    [HttpPost(ApiRoutes.Order.Refund)]
    public async Task<IActionResult> RefundOrder([FromRoute] Guid id)
    {
        var command = _mapper.Map<RefundOrderCommand>(id);
        var refundOrderResult = await _mediator.Send(command);

        return refundOrderResult.Match(
            success => NoContent(),
            errors => Problem(errors));
    }
}

