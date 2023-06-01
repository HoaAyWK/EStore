using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.ProductAttributes.Queries.GetProductAttributeById;

public class GetProductAttributeByIdQueryHandler
    : IRequestHandler<GetProductAttributeByIdQuery, ErrorOr<ProductAttribute>>
{
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;
    public GetProductAttributeByIdQueryHandler(
        IProductAttributeReadRepository productAttributeReadRepository)
    {
        _productAttributeReadRepository = productAttributeReadRepository;
    }

    public async Task<ErrorOr<ProductAttribute>> Handle(GetProductAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        var productAttribute = await _productAttributeReadRepository.GetByIdAsync(
            ProductAttributeId.Create(new Guid(request.ProductAttributeId)));

        if (productAttribute is null)
        {
            return Errors.ProductAttribute.NotFound;
        }

        return productAttribute;
    }
}
