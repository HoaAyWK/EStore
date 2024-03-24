using EStore.Application.Discounts.Services;
using EStore.Contracts.Discount;
using MediatR;

namespace EStore.Application.Discounts.Queries.GetDiscountListPaged;

public class GetDiscountListPagedQueryHandler
    : IRequestHandler<GetDiscountListPagedQuery, ListPagedDiscountResponse>
{
    private readonly IDiscountReadService _discountReadService;

    public GetDiscountListPagedQueryHandler(IDiscountReadService discountReadService)
    {
        _discountReadService = discountReadService;
    }

    public async Task<ListPagedDiscountResponse> Handle(
        GetDiscountListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _discountReadService.GetListPagedAsync(request.PageSize, request.Page);        
    }
}
