using ErrorOr;
using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler
    : IRequestHandler<GetBrandByIdQuery, ErrorOr<Brand>>
{
    private readonly IBrandReadRepository _brandReadRepository;

    public GetBrandByIdQueryHandler(IBrandReadRepository brandReadRepository)
    {
        _brandReadRepository = brandReadRepository;
    }

    public async Task<ErrorOr<Brand>> Handle(
        GetBrandByIdQuery request,
        CancellationToken cancellationToken)
    {
        var brand = await _brandReadRepository.GetByIdAsync(request.BrandId);

        if (brand is null)
        {
            return Errors.Brand.NotFound;
        }

        return brand;
    }
}
