namespace EStore.Contracts.Orders;

public record OrderResponse(
    Guid Id,
    long OrderNumber,
    Guid CustomerId,
    string OrderStatus,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime,
    decimal TotalAmount,
    string PaymentMethod,
    OrderResponse.ShippingAddressResponse ShippingAddress,
    List<OrderResponse.OrderItemResponse> OrderItems)
{
    public record ShippingAddressResponse(
        string ReceiverName,
        string PhoneNumber,
        string Street,
        string City,
        string State,
        string Country,
        string ZipCode);

    public record OrderItemResponse(
        Guid ProductId,
        Guid? ProductVariantId,
        string ProductName,
        string ProductImage,
        string? ProductAttributes,
        decimal UnitPrice,
        decimal SubTotal,
        decimal TotalDiscount,
        int Quantity);
}
