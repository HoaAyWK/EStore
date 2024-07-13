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
        decimal BasePrice,
        decimal FinalPrice,
        string? ProductImageUrl,
        CartItemResponse.DiscountResponse? Discount,
        int Quantity,
        decimal SubTotal,
        int StockQuantity)
    {
        public record DiscountResponse(
            bool UseDiscountPercentage,
            decimal Percentage,
            decimal Amount);
    }
};
