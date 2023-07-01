using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.Repositories;
using MediatR;

namespace EStore.Application.ProductVariants.Events;

public class ProductAttributeValueUpdatedDomainEventHandler
    : INotificationHandler<ProductAttributeValueUpdatedDomainEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductAttributeValueUpdatedDomainEventHandler(
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProductAttributeValueUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.OldPrice == notification.NewPrice)
        {
            return;
        }

        var productVariants = await _productVariantRepository.GetByProductIdAsync(notification.ProductId);

        foreach (var variant in productVariants)
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
