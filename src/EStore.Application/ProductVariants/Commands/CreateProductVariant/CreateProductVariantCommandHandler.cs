using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate;
using EStore.Domain.ProductVariantAggregate.Repositories;
using MediatR;

namespace EStore.Application.ProductVariants.Commands.CreateProductVariant;

public class CreateProductVariantCommandHandler
    : IRequestHandler<CreateProductVariantCommand, ErrorOr<ProductVariant>>
{
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductVariantCommandHandler(
        IProductVariantRepository productVariantRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ProductVariant>> Handle(
        CreateProductVariantCommand request,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            errors.Add(Errors.ProductVariant.ProductNotFound(request.ProductId));
        }

        var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(null);

        foreach (var selectedAttribute in request.SelectedAttributes)
        {
            attributeSelection.AddAttributeValue(
                selectedAttribute.ProductAttributeId,
                selectedAttribute.ProductAttributeValueId);
        }

        var productVariants = await _productVariantRepository.GetAllAsync();
        
       
        // Check if product variant already existed.
        foreach (var variant in productVariants)
        {
            var existingSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                .Create(variant.RawAttributeSelection);

            if (existingSelection.Equals(attributeSelection))
            {
                errors.Add(Errors.ProductVariant.DuplicateVariant);
                break;
            }
        }

         decimal variantPrice = 0;
        // Check if product attribute value already existed, if not add error to errors list
        foreach (var selection in request.SelectedAttributes)
        {
            int isDuplicateAttribute = request.SelectedAttributes
                .Where(x => x.ProductAttributeId == selection.ProductAttributeId)
                .Count();
            
            var attribute = product!.ProductAttributes.FirstOrDefault(
                x => x.Id == selection.ProductAttributeId);

            if (attribute is null)
            {
                errors.Add(Errors.Product.ProductAttributeNotFound);
            }

            if (!attribute!.CanCombine)
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

        var createProductVariantResult = ProductVariant.Create(
            request.ProductId,
            request.StockQuantity,
            variantPrice,
            request.AssignedProductImageIds ?? "",
            attributeSelection.AsJson(),
            request.IsActive);

        if (createProductVariantResult.IsError)
        {
            errors.AddRange(createProductVariantResult.Errors);
        }

        var productVariant = createProductVariantResult.Value;

        if (errors.Count > 0)
        {
            return errors;
        }
        
        await _productVariantRepository.AddAsync(productVariant);
        await _unitOfWork.SaveChangesAsync();

        return productVariant;
    }
}
