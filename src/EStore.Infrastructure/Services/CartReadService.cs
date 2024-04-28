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
using EStore.Domain.ProductAggregate.Entities;

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
            cartItems.Sum(item => item.FinalPrice * item.Quantity),
            cartItems);
    }

    private async Task<List<CartResponse.CartItemResponse>> GetCartItemsAsync(Cart cart)
    {
        var productIds = cart.Items.Select(i => i.ProductId);

        var productsWithDiscountQuery =
            from product in _dbContext.Products.AsNoTracking()
            join discount in _dbContext.Discounts.AsNoTracking()
            on product.DiscountId equals discount.Id into pds
            from pd in pds.DefaultIfEmpty()
            where productIds.Contains(product.Id)
            select new
            {
                Product = product,
                Discount = pd
            };
            
        var productsWithDiscount = await productsWithDiscountQuery.ToListAsync();

        List<CartResponse.CartItemResponse> itemResponses = cart.Items.Select(cartItem =>
        {
            var productWithDiscount = productsWithDiscount
                .Where(p => p.Product.Id == cartItem.ProductId)
                .First();

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

            CartResponse.CartItemResponse.DiscountResponse? discountResponse = null;

            if (productWithDiscount.Discount is not null)
            {
                discountResponse = new CartResponse.CartItemResponse.DiscountResponse(
                    productWithDiscount.Discount.UsePercentage,
                    productWithDiscount.Discount.DiscountPercentage,
                    productWithDiscount.Discount.DiscountAmount);
            }

            ProductVariant? productVariant = null;

            if (cartItem.ProductVariantId is not null)
            {
                productVariant = productWithDiscount.Product.ProductVariants
                    .FirstOrDefault(v => v.Id == cartItem.ProductVariantId);
            }

            var basePrice = _priceCalculationService.CalculatePrice(
                productWithDiscount.Product,
                productVariant);

            var finalPrice = basePrice;

            if (productWithDiscount.Discount is not null)
            {
                finalPrice = _priceCalculationService.ApplyDiscount(
                    finalPrice,
                    productWithDiscount.Discount);
            }

            var productImage = productWithDiscount.Product.Images
                .Where(image => image.IsMain)
                .FirstOrDefault();

            string? imageUrl = productImage?.ImageUrl;

            if (productVariant is not null)
            {
                var productVariantImages = productVariant.AssignedProductImageIds.Split(' ');

                if (productVariantImages.Length > 0 && !productVariantImages.Contains(imageUrl))
                {
                    var firstImageId = productVariantImages.First();

                    var image = productWithDiscount.Product.Images
                        .FirstOrDefault(i => i.Id == ProductImageId.Create(new Guid(firstImageId)));

                    if (image is not null)
                    {
                        imageUrl = image.ImageUrl;
                    }
                }
            }

            return new CartResponse.CartItemResponse(
                cartItem.Id.Value,
                cartItem.ProductId.Value,
                cartItem.ProductVariantId?.Value,
                productWithDiscount.Product.Name,
                productAttributesSb.ToString(),
                basePrice,
                finalPrice,
                imageUrl,
                discountResponse,
                cartItem.Quantity,
                finalPrice * cartItem.Quantity);
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
            on product.DiscountId equals discount.Id into pds
            from pd in pds.DefaultIfEmpty()
            where productIds.Contains(product.Id)
            select new
            {
                Product = product,
                Discount = pd
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

            ProductVariant? variant = null;

            if (cartItem.ProductVariantId is not null)
            {
                variant = productWithDiscount.Product.ProductVariants
                    .FirstOrDefault(v => v.Id == cartItem.ProductVariantId);
            }

            var price = _priceCalculationService.CalculatePrice(
                productWithDiscount.Product,
                variant);

            if (productWithDiscount.Discount is not null)
            {
                price = _priceCalculationService.ApplyDiscount(
                    price,
                    productWithDiscount.Discount);
            }

            totalAmount += price * cartItem.Quantity;
        }

        // if (totalAmount != cartTotalAmount)
        // {
        //     return Errors.Cart.InvalidTotalAmount;
        // }

        return Result.Success;
    }
}
