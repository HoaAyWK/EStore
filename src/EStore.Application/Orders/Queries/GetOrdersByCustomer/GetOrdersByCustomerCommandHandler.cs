using ErrorOr;
using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerCommandHandler
    : IRequestHandler<GetOrdersByCustomerCommand, PagedList<Order>>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrdersByCustomerCommandHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<PagedList<Order>> Handle(GetOrdersByCustomerCommand request, CancellationToken cancellationToken)
    {
        return await _orderReadService.GetByCustomerIdAsync(
            request.CustomerId,
            request.Page,
            request.PageSize);
    }
}
