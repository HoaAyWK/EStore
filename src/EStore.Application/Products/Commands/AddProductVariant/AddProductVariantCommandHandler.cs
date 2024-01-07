using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductVariant;

public class AddProductVariantCommandHandler : IRequestHandler<AddProductVariantCommand, ErrorOr<ProductVariant>>
{
    private readonly IProductRepository _productRepository;

    public AddProductVariantCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<ProductVariant>> Handle(AddProductVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var errors = new List<Error>();
        var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(null);

        foreach (var selectedAttribute in request.SelectedAttributes)
        {
            attributeSelection.AddAttributeValue(
                selectedAttribute.ProductAttributeId,
                selectedAttribute.ProductAttributeValueId);
        }

        foreach (var variant in product.ProductVariants)
        {
            var existingSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                .Create(variant.RawAttributeSelection);

            if (existingSelection.Equals(attributeSelection))
            {
                errors.Add(Errors.Product.DuplicateVariant);
                break;
            }
        }

        decimal variantPrice = 0;
        
        // Check if product attribute value already existed, if not add error to errors list
        foreach (var selection in request.SelectedAttributes)
        {   
            var attribute = product.ProductAttributes.FirstOrDefault(
                x => x.Id == selection.ProductAttributeId);

            if (attribute is null)
            {
                errors.Add(Errors.Product.ProductAttributeNotFound);
            }
            else
            {
                if (!attribute.CanCombine)
                {
                    errors.Add(Errors.Product.ProductAttributeCannotCombine);
                }

                var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
                    x => x.Id == selection.ProductAttributeValueId);

                if (attributeValue is null)
                {
                    errors.Add(Errors.Product.ProductAttributeValueNotFound);
                }
                else
                {
                    variantPrice += attributeValue.PriceAdjustment;
                }
            }
        }

        var createProductVariantResult = ProductVariant.Create(
            request.StockQuantity,
            variantPrice,
            request.AssignedProductImageIds ?? "",
            attributeSelection.AsJson(),
            request.IsActive);

        if (createProductVariantResult.IsError)
        {
            errors.AddRange(createProductVariantResult.Errors);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var productVariant = createProductVariantResult.Value;

        product.AddProductVariant(productVariant);

        return productVariant;
    }
}
