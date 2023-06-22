using ErrorOr;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Application.Products.Dtos;
using EStore.Application.Products.Services;

namespace EStore.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductDto>>
{
    private readonly IProductReadService _productReadService;
    public GetProductByIdQueryHandler(IProductReadService productReadService)
    {
        _productReadService = productReadService;
    }

    public async Task<ErrorOr<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productReadService.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        return product;
    }
}
