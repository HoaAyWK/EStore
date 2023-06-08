using System.Text;
using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.AddVariant;

public class AddCommandHandler
    : IRequestHandler<AddVariantCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Product>> Handle(
        AddVariantCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        if (request.AttributeSelections.Count != product.ProductAttributes.Count)
        {
            return Errors.Product.InvalidAttributeSelectionNumbers;
        }

        foreach (var selection in request.AttributeSelections)
        {
            int isDuplicateAttribute = request.AttributeSelections
                .Where(x => x.ProductAttributeId == selection.ProductAttributeId)
                .Count();
            
            var attribute = product.ProductAttributes.FirstOrDefault(
                x => x.Id == selection.ProductAttributeId);

            if (attribute is null)
            {
                return Errors.Product.ProductAttributeNotFound;
            }

            if (!attribute.CanCombine)
            {
                return Errors.Product.ProductAttributeCannotCombine;
            }

            var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
                x => x.Id == selection.ProductAttributeValueId);

            if (attributeValue is null)
            {
                return Errors.Product.ProductAttributeValueNotFound;
            }
        }

        StringBuilder assignedImageIds = new StringBuilder("");

        if (request.AssignedImageIds is not null)
        {
            for (int i = 0; i < request.AssignedImageIds.Count - 1; i++)
            {
                var assignedId = request.AssignedImageIds.ElementAt(i);

                var productImageId = product.Images.FirstOrDefault(
                    image => image.Id == ProductImageId.Create(assignedId));

                if (productImageId is null)
                {
                    return Errors.Product.ProductImageNotFound;
                }
                
                if (i is 0)
                {
                    assignedImageIds.Append(assignedId.ToString().ToLower());
                }
                else
                {
                    assignedImageIds.Append($" {assignedId.ToString().ToLower()}");
                }
            }
        }

        for (int i = 0; i < request.AttributeSelections.Count - 1; i++)
        {
            var leftAttribute = product.ProductAttributes.FirstOrDefault(
                x => x.Id == request.AttributeSelections[i].ProductAttributeId);
            
            var leftAttributeValue = leftAttribute!.ProductAttributeValues.FirstOrDefault(
                x => x.Id == request.AttributeSelections[i].ProductAttributeValueId);

            for (int j = request.AttributeSelections.Count - 1; j > i; j--)
            {
                var rightAttribute = product.ProductAttributes.FirstOrDefault(
                    x => x.Id == request.AttributeSelections[j].ProductAttributeId);

                var rightAttributeValue = rightAttribute!.ProductAttributeValues.FirstOrDefault(
                    x => x.Id == request.AttributeSelections[j].ProductAttributeValueId);

                leftAttributeValue!.CombinedAttributes!.AddAttributeValue(
                    request.AttributeSelections[i].ProductAttributeId,
                    request.AttributeSelections[j].ProductAttributeValueId);

                rightAttributeValue!.CombinedAttributes!.AddAttributeValue(
                    request.AttributeSelections[j].ProductAttributeId,
                    request.AttributeSelections[i].ProductAttributeValueId);

                rightAttributeValue.UpdateRawConnectedAttributes(
                    rightAttributeValue!.CombinedAttributes!.AsJson());
            }

            leftAttributeValue!.UpdateRawConnectedAttributes(
                leftAttributeValue!.CombinedAttributes!.AsJson());
        }

        var attributeSelection = ProductAttributeSelection.Create(null);

        foreach (var selection in request.AttributeSelections)
        {
            attributeSelection.AddAttributeValue(
                selection.ProductAttributeId,
                selection.ProductAttributeValueId);
        }

        foreach (var combination in product.ProductVariants)
        {
            if (attributeSelection.Equals(combination.AttributeSelection))
            {
                return Errors.Product.ProductVariantExisted;
            }
        }

        var productVariant = ProductVariant.Create(
            stockQuantity: request.StockQuantity,
            price: request.Price,
            assignedProductImageIds: assignedImageIds.ToString(),
            isActive: request.IsActive ?? false);
        
        productVariant.UpdateRawAttributes(attributeSelection.AsJson());
        product.AddProductVariant(productVariant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }
}
