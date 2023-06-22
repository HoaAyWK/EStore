using EStore.Application.Products.Dtos;
using EStore.Application.Products.Services;
using EStore.Contracts.Common;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductListPaged;

public class GetProductListPagedQueryHandler
    : IRequestHandler<GetProductListPagedQuery, PagedList<ProductDto>>
{
    private readonly IProductReadService _productReadService;

    public GetProductListPagedQueryHandler(IProductReadService productReadService)
    {
        _productReadService = productReadService;
    }

    public async Task<PagedList<ProductDto>> Handle(
        GetProductListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _productReadService.GetProductListPagedAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize);
    }
}
