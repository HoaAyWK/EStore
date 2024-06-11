namespace EStore.Contracts.Invoices;

public class InvoiceResponse
{
    public Guid Id { get; set; }
    
    public long InvoiceNumber { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
    
    public DateTime UpdatedDateTime { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public string PaymentMethod { get; set; } = null!;
    
    public string PaymentStatus { get; set; } = null!;
    
    public CustomerResponse? Customer { get; set; }

    public ShippingAddressResponse ShippingAddress { get; set; } = null!;

    public List<OrderItemResponse> OrderItems { get; set; } = new();
}

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

public record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? AvatarUrl);
