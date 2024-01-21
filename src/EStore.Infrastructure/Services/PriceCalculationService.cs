using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Utilities;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Infrastructure.Services;

public class PriceCalculationService : IPriceCalculationService
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public PriceCalculationService(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public decimal CalculatePrice(
        Product product,
        ProductVariant? productVariant)
    {
        decimal price = product.Price;

        if (product.HasVariant && productVariant is not null)
        {
            if (productVariant.Price is not null)
            {
                price = productVariant.Price.Value;
            }
            else
            {
                var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                .Create(productVariant.RawAttributeSelection);

                foreach (var selection in attributeSelection.AttributesMap)
                {
                    var attribute = product.ProductAttributes.FirstOrDefault(
                        a => a.Id == selection.Key);

                    if (attribute is null)
                    {
                        continue;
                    }

                    var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
                        v => v.Id == selection.Value.First());

                    if (attributeValue is null)
                    {
                        continue;
                    }

                    price += attributeValue.PriceAdjustment;
                }
            }
        }

        return price;
    }

    public decimal ApplyDiscount(decimal price, Discount discount)
    {
        if (IsDiscountValid(discount))
        {
            if (discount.UsePercentage)
            {
                price -= price * discount.DiscountPercentage;
            }
            else
            {
                price -= discount.DiscountAmount;
            }
        }

        return price;
    }

    private bool IsDiscountValid(Discount discount)
    {
        return _dateTimeProvider.UtcNow >= discount.StartDateTime &&
            _dateTimeProvider.UtcNow <= discount.EndDateTime;
    }
}
