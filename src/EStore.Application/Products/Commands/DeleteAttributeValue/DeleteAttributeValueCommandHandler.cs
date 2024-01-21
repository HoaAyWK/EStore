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

        var removedAttributeValueResult = product.RemoveAttributeValue(
            request.ProductAttributeId,
            request.ProductAttributeValueId);

        if (removedAttributeValueResult.IsError)
        {
            return removedAttributeValueResult.Errors;
        }

        return product;
    }
}
