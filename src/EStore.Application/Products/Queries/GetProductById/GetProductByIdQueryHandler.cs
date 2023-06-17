using ErrorOr;
using EStore.Domain.ProductAggregate;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;

namespace EStore.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ErrorOr<Product>>
{
    private readonly IProductReadRepository _productReadRepository;

    public GetProductByIdQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        return product;
    }
}
