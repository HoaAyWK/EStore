using EStore.Domain.Common.Models;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate.Entities;

public sealed class OrderItem : Entity<OrderItemId>
{
    public ItemOrdered ItemOrdered { get; private set; } = null!;

    public decimal UnitPrice { get; private set; }

    public decimal DiscountAmount { get; private set; }

    public int Quantity { get; private set; }

    public decimal SubTotal => UnitPrice * Quantity;

    public decimal TotalDiscount => DiscountAmount * Quantity;

    private OrderItem()
    {
    }

    private OrderItem(
        OrderItemId id,
        ItemOrdered itemOrdered,
        decimal unitPrice,
        decimal discountAmount,
        int quantity)
        : base(id)
    {
        ItemOrdered = itemOrdered;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        Quantity = quantity;
    }

    public static OrderItem Create(
        ItemOrdered itemOrdered,
        decimal unitPrice,
        decimal discountAmount,
        int quantity)
    {
        return new(
            OrderItemId.CreateUnique(),
            itemOrdered,
            unitPrice,
            discountAmount,
            quantity);
    }
}
