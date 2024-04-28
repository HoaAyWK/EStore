using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate;

public sealed class Order : AggregateRoot<OrderId>, IAuditableEntity, ISoftDeletableEntity
{
    
    private readonly List<OrderItem> _orderItems = new();
    private readonly List<OrderStatusHistoryTracking> _orderStatusHistoryTrackings = new();

    public long OrderNumber { get; private set; }

    public CustomerId CustomerId { get; private set; } = null!;

    public OrderStatus OrderStatus { get; private set; } = null!;

    public string? TransactionId { get; private set; }

    public ShippingAddress ShippingAddress { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; } = PaymentMethod.CashOnDelivery;

    public decimal TotalAmount => _orderItems.Sum(item => item.SubTotal);

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    
    public IReadOnlyList<OrderStatusHistoryTracking> OrderStatusHistoryTrackings
        => _orderStatusHistoryTrackings.AsReadOnly();

    private Order()
    {
    }

    private Order(
        OrderId id,
        long orderNumber,
        CustomerId customerId,
        OrderStatus orderStatus,
        string? transactionId,
        ShippingAddress shippingAddress,
        List<OrderItem> orderItems,
        DateTime orderedDateTime,
        PaymentMethod paymentMethod)
        : base(id)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderStatus = orderStatus;
        TransactionId = transactionId;
        ShippingAddress = shippingAddress;
        _orderItems = orderItems;
        PaymentMethod = paymentMethod;
        AddOrderStatusHistoryTracking(OrderStatusHistory.OrderPlaced, orderedDateTime);
    }

    public static Order Create(
        long orderNumber,
        CustomerId customerId,
        OrderStatus status,
        string? transactionId,
        ShippingAddress shippingAddress,
        List<OrderItem> orderItems,
        DateTime orderedDateTime,
        PaymentMethod paymentMethod)
    {
        var order = new Order(
            OrderId.CreateUnique(),
            orderNumber,
            customerId,
            status,
            transactionId,
            shippingAddress,
            orderItems,
            orderedDateTime,
            paymentMethod);

        // order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id, customerId));

        return order;
    }

    public void UpdateShippingAddress(ShippingAddress shippingAddress)
    {
        ShippingAddress = shippingAddress;
    }

    public void UpdateTransactionId(string transactionId)
    {
        TransactionId = transactionId;
    }

    public void UpdateOrderStatus(OrderStatus orderStatus)
    {
        OrderStatus = orderStatus;
    }

    public ErrorOr<OrderStatusHistoryTracking> AddOrderStatusHistoryTracking(
        OrderStatusHistory status,
        DateTime createdDateTime)
    {
        var latestOrderStatus = _orderStatusHistoryTrackings
            .OrderByDescending(x => x.CreatedDateTime)
            .FirstOrDefault();

        if (latestOrderStatus is not null && status.Value <= latestOrderStatus.Status.Value)
        {
            return Errors.Order.InvalidOrderStatusHistory;
        }

        var orderStatusHistoryTracking = OrderStatusHistoryTracking.Create(status, createdDateTime);

        _orderStatusHistoryTrackings.Add(orderStatusHistoryTracking);

        return orderStatusHistoryTracking;
    }

    public void MarkAsRefunded()
    {
        OrderStatus = OrderStatus.Refunded;
        RaiseDomainEvent(new OrderRefundedDomainEvent(Id));
    }
}
