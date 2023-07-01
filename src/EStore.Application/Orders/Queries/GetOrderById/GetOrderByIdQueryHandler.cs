using ErrorOr;
using EStore.Application.Orders.Services;
using EStore.Domain.OrderAggregate;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, ErrorOr<Order>>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrderByIdQueryHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<ErrorOr<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderReadService.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        return order;
    }
}
