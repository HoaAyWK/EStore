using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductAttributeValue;

public class AddProductAttributeValueCommandHandler
    : IRequestHandler<AddProductAttributeValueCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;

    public AddProductAttributeValueCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        AddProductAttributeValueCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var addAttributeValueResult = product.AddProductAttributeValue(
            request.ProductAttributeId,
            request.Name,
            request.PriceAdjustment,
            request.Color,
            request.DisplayOrder);

        if (addAttributeValueResult.IsError)
        {
            return addAttributeValueResult.Errors;
        }
        
        return product;
    }
}
