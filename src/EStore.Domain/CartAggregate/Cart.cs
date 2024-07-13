using ErrorOr;
using EStore.Domain.CartAggregate.Entities;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.Common.Models;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.CartAggregate;

public sealed class Cart : AggregateRoot<CartId>
{
    private readonly List<CartItem> _items = new();

    public CustomerId CustomerId { get; private set; }

    public decimal TotalPrice => _items.Sum(i => i.Quantity * i.UnitPrice);

    public int TotalItems => _items.Sum(i => i.Quantity);

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    private Cart(CartId id, CustomerId customerId) : base(id)
    {
        CustomerId = customerId;
    }
    
    public static Cart Create(CustomerId customerId)
    {
        return new(CartId.CreateUnique(), customerId);
    }

    public ErrorOr<Success> AddItem(
        ProductId productId,
        ProductVariantId? productVariantId,
        decimal unitPrice,
        int stockQuantity,
        int quantity = 1)
    {
        var existingItem = Items.FirstOrDefault(i =>
            i.ProductId == productId &&
            i.ProductVariantId! == productVariantId!);


        if (existingItem is not null)
        {
            existingItem.AddQuantity(quantity);

            if (existingItem.Quantity <= 0)
            {
                _items.Remove(existingItem);
            }

            if (existingItem.Quantity > stockQuantity)
            {
                return Errors.Cart.ProductQuantityExceedsStock;
            }

            return Result.Success;
        }

        var createCartItemResult = CartItem.Create(
            quantity,
            unitPrice,
            productId,
            productVariantId);

        if (createCartItemResult.IsError)
        {
            return createCartItemResult.Errors;
        }

        var item = createCartItemResult.Value;

        if (item.Quantity > stockQuantity)
        {
            return Errors.Cart.ProductQuantityExceedsStock;
        }

        _items.Add(item);

        return Result.Success;
    }

    public void Clear()
    {
        _items.Clear();
    }

    public void RemoveItem(CartItemId itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);

        if (item is null)
        {
            return;
        }

        _items.Remove(item);
    }

    public void UpdateCustomerId(CustomerId customerId)
    {
        CustomerId = customerId;
    }
}
