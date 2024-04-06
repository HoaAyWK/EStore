using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttribute;

public class UpdateProductAttributeCommandHandler
    : IRequestHandler<UpdateProductAttributeCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductAttributeCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        UpdateProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var updateProductAttributeResult = product.UpdateProductAttribute(
            request.Id,
            request.Name,
            request.CanCombine,
            request.DisplayOrder);

        if (updateProductAttributeResult.IsError)
        {
            return updateProductAttributeResult.Errors;
        }
            
        return product;
    }
}
