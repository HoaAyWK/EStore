using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;
using EStore.Domain.ProductAggregate.Entities;

namespace EStore.Application.Products.Commands.AddProductAttribute;

public class AddProductAttributeCommandHandler
    : IRequestHandler<AddProductAttributeCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;

    public AddProductAttributeCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        AddProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var addProductAttributeResult = product.AddProductAttribute(
            request.Name,
            request.CanCombine,
            request.Colorable,
            request.DisplayOrder);

        if (addProductAttributeResult.IsError)
        {
            return addProductAttributeResult.Errors;
        }
        
        return product;
    }
}
