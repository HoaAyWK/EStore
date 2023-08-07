using System.Text;
using ErrorOr;
using EStore.Application.Carts.Services;
using EStore.Contracts.Carts;
using EStore.Domain.CartAggregate;
using EStore.Domain.Common.Utilities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EStore.Application.Common.Interfaces.Services;

namespace EStore.Infrastructure.Services;

internal sealed class CartReadService : ICartReadService
{
    private readonly EStoreDbContext _dbContext;
    private readonly IPriceCalculationService _priceCalculationService;

    public CartReadService(
        EStoreDbContext dbContext,
        IPriceCalculationService priceCalculationService)
    {
        _dbContext = dbContext;
        _priceCalculationService = priceCalculationService;
    }

    public async Task<CartResponse?> GetByCustomerIdAsync(CustomerId customerId)
    {
        var cart = await _dbContext.Carts
            .AsNoTracking()
            .Where(cart => cart.CustomerId == customerId)
            .FirstOrDefaultAsync();

        if (cart is null)
        {
            return null;
        }

        var cartItems = await GetCartItemsAsync(cart);

        return new CartResponse(
            cart.Id.Value,
            cart.CustomerId.Value,
            cartItems.Sum(item => item.Price * item.Quantity),
            cartItems);
    }

    private async Task<List<CartResponse.CartItemResponse>> GetCartItemsAsync(Cart cart)
    {
        var productIds = cart.Items.Select(i => i.ProductId);

        var productsWithDiscountQuery =
            from product in _dbContext.Products.AsNoTracking()
            join discount in _dbContext.Discounts.AsNoTracking()
            on product.DiscountId equals discount.Id
            where productIds.Contains(product.Id)
            select new
            {
                Product = product,
                Discount = discount
            };
            
        var productsWithDiscount = await productsWithDiscountQuery.ToListAsync();

        List<CartResponse.CartItemResponse> itemResponses = cart.Items.Select(cartItem =>
        {
            var productWithDiscount = productsWithDiscount.First(p => p.Product.Id == cartItem.ProductId);
            string? rawAttributeSelection = null;

            if (cartItem.ProductVariantId is not null)
            {
                var variant = productWithDiscount.Product.ProductVariants
                    .FirstOrDefault(v => v.Id == cartItem.ProductVariantId);

                if (variant is not null)
                {
                    rawAttributeSelection = variant.RawAttributeSelection;
                }
            }

            StringBuilder productAttributesSb = new();

            if (rawAttributeSelection is not null)
            {
                var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                    .Create(rawAttributeSelection);

                var lastAttribute = productWithDiscount.Product.ProductAttributes.Last();

                foreach (var selection in attributeSelection.AttributesMap)
                {
                    var attribute = productWithDiscount.Product.ProductAttributes
                        .FirstOrDefault(a => a.Id == selection.Key);

                    if (attribute is not null)
                    {
                        foreach (var value in selection.Value)
                        {
                            var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(v => v.Id == value);

                            if (attributeValue is not null)
                            {
                                productAttributesSb.Append($"{attribute.Name}: {attributeValue.Name}");
                            }
                        }

                        if (attribute.Id != lastAttribute.Id)
                        {
                            productAttributesSb.Append(" - ");
                        }
                    }
                }
            }

            var discountResponse = new CartResponse.CartItemResponse.DiscountResponse(
                productWithDiscount.Discount.UsePercentage,
                productWithDiscount.Discount.DiscountPercentage,
                productWithDiscount.Discount.DiscountAmount);

            var price = _priceCalculationService.CalculatePrice(
                productWithDiscount.Product,
                cartItem.ProductVariantId,
                productWithDiscount.Discount);

            return new CartResponse.CartItemResponse(
                cartItem.Id.Value,
                cartItem.ProductId.Value,
                cartItem.ProductVariantId?.Value,
                productWithDiscount.Product.Name,
                productAttributesSb.ToString(),
                price,
                discountResponse,
                cartItem.Quantity,
                price * cartItem.Quantity);
        }).ToList();

        return itemResponses;
    }


    public async Task<ErrorOr<Success>> ValidatePurchasedItemsAsync(CustomerId customerId, decimal cartTotalAmount)
    {
        var cart = await _dbContext.Carts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        var productIds = cart.Items.Select(item => item.ProductId);

        var productsWithDiscountQuery =
            from product in _dbContext.Products.AsNoTracking()
            join discount in _dbContext.Discounts.AsNoTracking()
            on product.DiscountId equals discount.Id
            where productIds.Contains(product.Id)
            select new
            {
                Product = product,
                Discount = discount
            };
            
        decimal totalAmount = 0;
        var productsWithDiscount = await productsWithDiscountQuery.ToListAsync();

        foreach (var cartItem in cart.Items)
        {
            var productWithDiscount = productsWithDiscount.FirstOrDefault(
                x => x.Product.Id == cartItem.ProductId);

            if (productWithDiscount is null)
            {
                return Errors.Cart.ProductNotFound(cartItem.ProductId);
            }

            if (cartItem.ProductVariantId is null)
            {
                if (productWithDiscount.Product.StockQuantity < cartItem.Quantity)
                {
                    return Errors.Cart.ProductOutOfStock(productWithDiscount.Product.Id);
                }
            }
            else
            {
                var productVariant = productWithDiscount.Product.ProductVariants
                    .FirstOrDefault(v => v.Id == cartItem.ProductVariantId);

                if (productVariant is null)
                {
                    return Errors.Cart.ProductVariantNotFound(cartItem.ProductVariantId);
                }

                if (productVariant.StockQuantity < cartItem.Quantity)
                {
                    return Errors.Cart.ProductVariantOutOfStock(productVariant.Id);
                }
            }

            var price = _priceCalculationService.CalculatePrice(
                productWithDiscount.Product,
                cartItem.ProductVariantId,
                productWithDiscount.Discount);

            totalAmount += price * cartItem.Quantity;
        }

        if (totalAmount != cartTotalAmount)
        {
            return Errors.Cart.InvalidTotalAmount;
        }

        return Result.Success;
    }
}
