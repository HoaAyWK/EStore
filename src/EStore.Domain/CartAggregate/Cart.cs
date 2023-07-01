using ErrorOr;
using EStore.Domain.CartAggregate.Entities;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

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
        int quantity = 1)
    {
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

        var existingItem = Items.FirstOrDefault(i =>
            i.ProductId == productId &&
            i.ProductVariantId! == productVariantId!);

        if (existingItem is null)
        {
            _items.Add(item);
            
            return Result.Success;
        }

        var addQuantityResult = existingItem.AddQuantity(quantity);

        if (addQuantityResult.IsError)
        {
            return addQuantityResult.Errors;
        }

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
