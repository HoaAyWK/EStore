using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;
using EStore.Domain.ProductAggregate.Entities;

namespace EStore.Application.Products.Commands.AddProductAttributeValue;

public class AddProductAttributeValueCommandHandler
    : IRequestHandler<AddProductAttributeValueCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductAttributeValueCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
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

        var productAttribute = product.ProductAttributes.FirstOrDefault(
            x => x.Id == request.ProductAttributeId);

        if (productAttribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        var productAttributeValue = ProductAttributeValue.Create(
            request.Name,
            request.PriceAdjustment ?? 0,
            request.Alias);

        productAttribute.AddAttributeValue(productAttributeValue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return product;
    }
}
