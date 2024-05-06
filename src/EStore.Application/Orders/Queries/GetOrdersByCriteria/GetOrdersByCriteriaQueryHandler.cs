using EStore.Application.Orders.Services;
using EStore.Contracts.Orders;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCriteria;

public class GetOrdersByCriteriaQueryHandler :
    IRequestHandler<GetOrdersByCriteriaQuery, List<OrderResponse>>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrdersByCriteriaQueryHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<List<OrderResponse>> Handle(
        GetOrdersByCriteriaQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderReadService.GetOrdersByCriteriaAsync(
            request.CustomerId,
            request.ProductId,
            request.ProductVariantId);
    }
}
