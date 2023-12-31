using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Events;

public class ProductVariantCreatedDomainEventHandler
    : INotificationHandler<ProductVariantCreatedDomainEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductVariantCreatedDomainEventHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(ProductVariantCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            return;
        }

        var productVariant = product.ProductVariants.FirstOrDefault(v => v.Id == notification.ProductVariantId);

        if (productVariant is null)
        {
            return;
        }

        // TODO: add flag to indicate product variant is invalid by default
        // if everything goes right, mark product variant as valid

        var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
            .Create(productVariant.RawAttributeSelection);

        List<(ProductAttributeId, ProductAttributeValueId)> selections = new();

        foreach (var attribute in attributeSelection.AttributesMap)
        {
            selections.Add((attribute.Key, attribute.Value.First()));
        }

        for (int left = 0; left < selections.Count - 1; left++)
        {
            var leftAttribute = product.ProductAttributes
                .FirstOrDefault(x => x.Id == selections[left].Item1);

            if (leftAttribute is null)
            {
                return;
            }

            var leftAttributeValue = leftAttribute.ProductAttributeValues
                .FirstOrDefault(x => x.Id == selections[left].Item2);

            if (leftAttributeValue is null)
            {
                return;
            }

            var leftAttributeValueSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                .Create(leftAttributeValue.RawCombinedAttributes);

            for (int right = selections.Count - 1; right > left; right--)
            {
                var rightAttribute = product.ProductAttributes.FirstOrDefault(
                    x => x.Id == selections[right].Item1);

                if (rightAttribute is null)
                {
                    return;
                }

                var rightAttributeValue = rightAttribute.ProductAttributeValues.FirstOrDefault(
                    x => x.Id == selections[right].Item2);

                if (rightAttributeValue is null)
                {
                    return;
                }

                var rightAttributeValueSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                    .Create(rightAttributeValue.RawCombinedAttributes);

                leftAttributeValueSelection.AddAttributeValue(
                    selections[right].Item1,
                    selections[right].Item2);

                rightAttributeValueSelection.AddAttributeValue(
                    selections[left].Item1,
                    selections[left].Item2);

                rightAttributeValue.UpdateRawConnectedAttributes(
                    rightAttributeValueSelection.AsJson());
            }

            leftAttributeValue.UpdateRawConnectedAttributes(
                leftAttributeValueSelection.AsJson());
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
