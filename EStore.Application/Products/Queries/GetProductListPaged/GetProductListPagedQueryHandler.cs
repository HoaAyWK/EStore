using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductListPaged;

public class GetProductListPagedQueryHandler
    : IRequestHandler<GetProductListPagedQuery, ErrorOr<List<Product>>>
{
    private readonly IProductReadRepository _productReadRepository;

    public GetProductListPagedQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<ErrorOr<List<Product>>> Handle(
        GetProductListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _productReadRepository.GetAllWithBrandAndCategory();
    }
}
