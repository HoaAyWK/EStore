using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Brands;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler
    : IRequestHandler<GetBrandByIdQuery, ErrorOr<BrandResponse>>
{
    private readonly IBrandReadService _brandReadService;

    public GetBrandByIdQueryHandler(IBrandReadService brandReadService)
    {
        _brandReadService = brandReadService;
    }

    public async Task<ErrorOr<BrandResponse>> Handle(
        GetBrandByIdQuery request,
        CancellationToken cancellationToken)
    {
        var brand = await _brandReadService.GetByIdAsync(request.BrandId);

        if (brand is null)
        {
            return Errors.Brand.NotFound;
        }

        return brand;
    }
}
