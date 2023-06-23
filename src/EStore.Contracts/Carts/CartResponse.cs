namespace EStore.Contracts.Carts;

public record CartResponse(
    Guid Id,
    Guid CustomerId,
    List<CartResponse.CartItemResponse> Items)
{
    public record CartItemResponse(
        Guid Id,
        Guid ProductId,
        Guid? ProductVariantId,
        string ProductName,
        string? ProductAttributes,
        decimal ProductPrice,
        int Quantity);
};
