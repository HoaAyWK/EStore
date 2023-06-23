using System.Text;
using EStore.Application.Carts.Services;
using EStore.Contracts.Carts;
using EStore.Domain.CartAggregate;
using EStore.Domain.Common.Utilities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
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

        return new CartResponse(
            cart.Id.Value,
            cart.CustomerId.Value,
            await GetCartItemsAsync(cart));
    }

    private async Task<List<CartResponse.CartItemResponse>> GetCartItemsAsync(Cart cart)
    {
        var productIds = cart.Items.Select(i => i.ProductId);
        var productVariantIds = cart.Items.Select(i => i.ProductVariantId);

        var products = await _dbContext.Products
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        var productVariants = await _dbContext.ProductVariants
            .AsNoTracking()
            .Where(v => productVariantIds.Contains(v.Id))
            .Select(v => new
            {
                Id = v.Id,
                RawAttributeSelection = v.RawAttributeSelection
            })
            .ToListAsync();

        List<CartResponse.CartItemResponse> itemResponses = cart.Items.Select(cartItem =>
        {
            var product = products.First(p => p.Id == cartItem.ProductId);
            string? rawAttributeSelection = null;

            if (cartItem.ProductVariantId is not null)
            {
                var variant = productVariants.FirstOrDefault(v => v.Id == cartItem.ProductVariantId);

                if (variant is not null)
                {
                    rawAttributeSelection = variant.RawAttributeSelection;
                }
            }

            StringBuilder productAttributesSb = new StringBuilder();

            if (rawAttributeSelection is not null)
            {
                var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                    .Create(rawAttributeSelection);

                var lastAttribute = product.ProductAttributes.Last();

                foreach (var selection in attributeSelection.AttributesMap)
                {
                    var attribute = product.ProductAttributes.FirstOrDefault(a => a.Id == selection.Key);

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

            Guid? productVariantId = cartItem.ProductVariantId! != null!
                ? cartItem.ProductVariantId.Value
                : null;

            return new CartResponse.CartItemResponse(
                cartItem.Id.Value,
                cartItem.ProductId.Value,
                productVariantId,
                product.Name,
                productAttributesSb.ToString(),
                cartItem.UnitPrice,
                cartItem.Quantity);
        }).ToList();

        return itemResponses;
    }
}
