using ErrorOr;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.CartAggregate.Entities;

public sealed class CartItem : Entity<CartItemId>
{
    public const int MinQuantity = 1;
    
    public const decimal MinUnitPrice = 0;

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public ProductId ProductId { get; private set; }

    public ProductVariantId? ProductVariantId { get; private set; }

    private CartItem(
        int quantity,
        decimal unitPrice,
        ProductId productId,
        ProductVariantId productVariantId)
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        ProductId = productId;
        ProductVariantId = productVariantId;
    }
    
    public static ErrorOr<CartItem> Create(
        int quantity,
        decimal unitPrice,
        ProductId productId,
        ProductVariantId productVariantId)
    {
        var errors = new List<Error>();

        if (quantity < MinQuantity)
        {
            errors.Add(Errors.Cart.InvalidCartItemQuantity);
        }

        if (unitPrice < MinUnitPrice)
        {
            errors.Add(Errors.Cart.InvalidCartItemUnitPrice);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new CartItem(quantity, unitPrice, productId, productVariantId);
    }

    public ErrorOr<Success> AddQuantity(int quantity)
    {
        if (quantity < MinQuantity)
        {
            return Errors.Cart.InvalidCartItemQuantity;
        }

        Quantity += quantity;

        return Result.Success;
    }
}
