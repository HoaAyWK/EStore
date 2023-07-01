namespace EStore.Contracts.Orders;

public record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string OrderStatus,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime,
    decimal TotalAmount,
    OrderResponse.ShippingAddressResponse ShippingAddress,
    List<OrderResponse.OrderItemResponse> OrderItems)
{
    public record ShippingAddressResponse(
        string Address,
        string City,
        string State,
        string Country,
        string PostalCode);

    public record OrderItemResponse(
        Guid ProductId,
        Guid? ProductVariantId,
        string ProductName,
        string ProductImage,
        string? ProductAttributes,
        decimal UnitPrice,
        decimal SubTotal,
        int Quantity);
}
