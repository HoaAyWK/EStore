using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Brands;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandListPaged;

public class GetBrandListPagedQueryHandler
    : IRequestHandler<GetBrandListPagedQuery, ListPagedBrandResponse>
{
    private readonly IBrandReadService _brandReadService;

    public GetBrandListPagedQueryHandler(IBrandReadService brandReadService)
    {
        _brandReadService = brandReadService;
    }

    public async Task<ListPagedBrandResponse> Handle(GetBrandListPagedQuery request, CancellationToken cancellationToken)
    {
        return await _brandReadService.GetListPagedAsync(request.PageSize, request.Page);
    }
}
