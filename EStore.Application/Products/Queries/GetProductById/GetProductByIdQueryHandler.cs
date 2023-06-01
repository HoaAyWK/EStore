using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;
using EStore.Domain.Common.Errors;

namespace EStore.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        return product;
    }
}
