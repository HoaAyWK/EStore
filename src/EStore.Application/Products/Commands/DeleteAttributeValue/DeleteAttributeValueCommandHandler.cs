using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.DeleteAttributeValue;

public class DeleteAttributeValueCommandHandler
    : IRequestHandler<DeleteAttributeValueCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;

    public DeleteAttributeValueCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        DeleteAttributeValueCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var attribute = product.ProductAttributes.FirstOrDefault(
            x => x.Id == request.ProductAttributeId);

        if (attribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
            x => x.Id == request.ProductAttributeValueId);

        if (attributeValue is null)
        {
            return Errors.Product.ProductAttributeValueNotFound;
        }

        attribute.RemoveAttributeValue(attributeValue);

        return product;
    }
}
