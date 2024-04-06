using ErrorOr;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttributeValue;

public class UpdateProductAttributeValueCommandHandler
    : IRequestHandler<UpdateProductAttributeValueCommand, ErrorOr<Updated>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductAttributeValueCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateProductAttributeValueCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var attribute = product.ProductAttributes.FirstOrDefault(
            a => a.Id == request.ProductAttributeId);

        if (attribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
            v => v.Id == request.ProductAttributeValueId);

        if (attributeValue is null)
        {
            return Errors.Product.ProductAttributeValueNotFound;
        }

        if (product.HasVariant && product.ProductVariants.Count > 0)
        {
            return Errors.Product.ProductAlreadyHadVariants;
        }

        attributeValue.UpdateDetails(
            request.Name,
            request.Color,
            request.PriceAdjustment ?? 0,
            request.DisplayOrder);
        
        return Result.Updated;
    }
}
