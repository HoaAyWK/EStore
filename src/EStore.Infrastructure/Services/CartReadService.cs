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

namespace EStore.Infrastructure.Services;

internal sealed class CartReadService : ICartReadService
{
    private readonly EStoreDbContext _dbContext;

    public CartReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
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
        var totalAmountExcludeDiscount = cartItems.Sum(item => item.SubTotal);
        var TotalAmountIncludeDiscount = cartItems.Sum(item => item.SubTotal - item.SubDiscountTotal);
        var totalDiscountAmount = cartItems.Sum(item => item.SubDiscountTotal);

        return new CartResponse(
            cart.Id.Value,
            cart.CustomerId.Value,
            totalAmountExcludeDiscount,
            TotalAmountIncludeDiscount,
            totalDiscountAmount,
            cartItems);
    }

    private async Task<List<CartResponse.CartItemResponse>> GetCartItemsAsync(Cart cart)
    {
        var productIds = cart.Items.Select(i => i.ProductId);
        var productVariantIds = cart.Items.Select(i => i.ProductVariantId);

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
            decimal variantPrice = 0;

            if (cartItem.ProductVariantId is not null)
            {
                var variant = productWithDiscount.Product.ProductVariants
                    .FirstOrDefault(v => v.Id == cartItem.ProductVariantId);

                if (variant is not null)
                {
                    variantPrice = variant.Price ?? 0;
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

            decimal? specialPrice = null;

            if (productWithDiscount.Product.SpecialPrice.HasValue &&
                DateTime.UtcNow >= productWithDiscount.Product.SpecialPriceStartDateTime &&
                DateTime.UtcNow <= productWithDiscount.Product.SpecialPriceEndDateTime)
            {
                specialPrice = productWithDiscount.Product.SpecialPrice.Value;
            }

            Guid? productVariantId = cartItem.ProductVariantId! != null!
                ? cartItem.ProductVariantId.Value
                : null;
            
            CartResponse.CartItemResponse.DiscountResponse? discount = null;
            decimal discountSubTotal = 0;

            if (productWithDiscount.Discount is not null &&
                DateTime.UtcNow >= productWithDiscount.Discount.StartDateTime &&
                DateTime.UtcNow <= productWithDiscount.Discount.EndDateTime)
            {
                discount = new(
                    productWithDiscount.Discount.UsePercentage,
                    productWithDiscount.Discount.DiscountPercentage,
                    productWithDiscount.Discount.DiscountAmount);

                if (discount.UseDiscountPercentage)
                {
                    discountSubTotal = (productWithDiscount.Product.Price + variantPrice) * discount.Percentage * cartItem.Quantity;
                }
                else
                {
                    discountSubTotal = discount.Amount * cartItem.Quantity; 
                }
            }

            decimal subTotal = (productWithDiscount.Product.Price + variantPrice) * cartItem.Quantity;

            return new CartResponse.CartItemResponse(
                cartItem.Id.Value,
                cartItem.ProductId.Value,
                productVariantId,
                productWithDiscount.Product.Name,
                productAttributesSb.ToString(),
                cartItem.UnitPrice,
                specialPrice,
                discount,
                cartItem.Quantity,
                subTotal,
                discountSubTotal);
        }).ToList();

        return itemResponses;
    }


    public async Task<ErrorOr<Success>> ValidatePurchasedItemsAsync(CustomerId customerId)
    {
        var cart = await _dbContext.Carts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        foreach (var cartItem in cart.Items)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == cartItem.ProductId);

            if (product is null)
            {
                return Errors.Cart.ProductNotFound(cartItem.ProductId);
            }

            if (cartItem.ProductVariantId is null)
            {
                if (product.StockQuantity < cartItem.Quantity)
                {
                    return Errors.Cart.ProductOutOfStock(product.Id);
                }

                if (product.Price != cartItem.UnitPrice)
                {
                    return Errors.Cart.CartItemPriceChanged(cartItem.Id);
                }
            }
            else
            {
                var productVariant = product.ProductVariants
                    .FirstOrDefault(v => v.Id == cartItem.ProductVariantId);

                if (productVariant is null)
                {
                    return Errors.Cart.ProductVariantNotFound(cartItem.ProductVariantId);
                }

                if (productVariant.StockQuantity < cartItem.Quantity)
                {
                    return Errors.Cart.ProductVariantOutOfStock(productVariant.Id);
                }

                if (productVariant.Price + product.Price != cartItem.UnitPrice)
                {
                    return Errors.Cart.CartItemPriceChanged(cartItem.Id);
                }
            }

        }

        return Result.Success;
    }
}
