using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Queries.GetProductAttributeListPaged;

public class GetProductAttributeListPagedQueryHandler
    : IRequestHandler<GetProductAttributeListPagedQuery, ErrorOr<List<ProductAttribute>>>
{
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;

    public GetProductAttributeListPagedQueryHandler(
        IProductAttributeReadRepository productAttributeReadRepository)
    {
        _productAttributeReadRepository = productAttributeReadRepository;
    }

    public async Task<ErrorOr<List<ProductAttribute>>> Handle(
        GetProductAttributeListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _productAttributeReadRepository.GetAllAsync();
    }
}
