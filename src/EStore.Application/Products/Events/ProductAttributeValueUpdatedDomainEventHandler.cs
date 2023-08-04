using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Events;

public class ProductAttributeValueUpdatedDomainEventHandler
    : INotificationHandler<ProductAttributeValueUpdatedDomainEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductAttributeValueUpdatedDomainEventHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProductAttributeValueUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.OldPrice == notification.NewPrice)
        {
            return;
        }

        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            return;
        }

        foreach (var variant in product.ProductVariants)
        {
            var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                .Create(variant.RawAttributeSelection);

            foreach (var attribute in attributeSelection.AttributesMap)
            {
                if (notification.ProductAttributeId == attribute.Key)
                {
                    if (notification.ProductAttributeValueId == attribute.Value.First())
                    {
                        decimal amount = notification.NewPrice - notification.OldPrice;

                        variant.UpdatePrice(variant.Price + amount);
                    }
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
