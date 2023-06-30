using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate;

public sealed class Order : AggregateRoot<OrderId>, IAuditableEntity
{
    private readonly List<OrderItem> _orderItems = new();

    public CustomerId CustomerId { get; private set; } = null!;

    public OrderStatus OrderStatus { get; private set; } = null!;

    public string? TransactionId { get; private set; }

    public ShippingAddress ShippingAddress { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public decimal TotalAmount => _orderItems.Sum(item => item.SubTotal);

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order()
    {
    }

    private Order(
        OrderId id,
        CustomerId customerId,
        OrderStatus orderStatus,
        string? transactionId,
        ShippingAddress shippingAddress)
        : base(id)
    {
        CustomerId = customerId;
        OrderStatus = orderStatus;
        TransactionId = transactionId;
        ShippingAddress = shippingAddress;
    }

    public static Order Create(
        CustomerId customerId,
        OrderStatus status,
        string? transactionId,
        ShippingAddress shippingAddress)
    {
        return new(
            OrderId.CreateUnique(),
            customerId,
            status,
            transactionId,
            shippingAddress);
    }
}
