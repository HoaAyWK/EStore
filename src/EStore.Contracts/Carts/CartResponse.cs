namespace EStore.Contracts.Carts;

public record CartResponse(
    Guid Id,
    Guid CustomerId,
    decimal TotalAmountIncludeDiscount,
    List<CartResponse.CartItemResponse> Items)
{
    public record CartItemResponse(
        Guid Id,
        Guid ProductId,
        Guid? ProductVariantId,
        string ProductName,
        string? ProductAttributes,
        decimal Price,
        CartItemResponse.DiscountResponse? Discount,
        int Quantity,
        decimal SubTotal)
    {
        public record DiscountResponse(
            bool UseDiscountPercentage,
            decimal Percentage,
            decimal Amount);
    }
};
