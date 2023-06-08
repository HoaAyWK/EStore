using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.BrandAggregate.Repositories;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandListPaged;

public class GetBrandListPagedQueryHandler
    : IRequestHandler<GetBrandListPagedQuery, List<Brand>>
{
    private readonly IBrandReadRepository _brandReadRepository;

    public GetBrandListPagedQueryHandler(IBrandReadRepository brandReadRepository)
    {
        _brandReadRepository = brandReadRepository;
    }

    public async Task<List<Brand>> Handle(GetBrandListPagedQuery request, CancellationToken cancellationToken)
    {
        return await _brandReadRepository.GetAllAsync();
    }
}
