using ErrorOr;
using EStore.Application.Discounts.Services;
using EStore.Contracts.Discounts;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Discounts.Queries.GetDiscountByIdQuery;

public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, ErrorOr<DiscountResponse>>
{
    private readonly IDiscountReadService _discountReadService;

    public GetDiscountByIdQueryHandler(IDiscountReadService discountReadService)
    {
        _discountReadService = discountReadService;
    }

    public async Task<ErrorOr<DiscountResponse>> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await _discountReadService.GetDiscountByIdAsync(request.DiscountId);

        if (discount is null)
        {
            return Errors.Discount.NotFound;
        }

        return discount;
    }
}
