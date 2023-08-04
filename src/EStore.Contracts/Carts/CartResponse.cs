namespace EStore.Contracts.Carts;

public record CartResponse(
    Guid Id,
    Guid CustomerId,
    decimal TotalAmountExcludeDiscount,
    decimal TotalAmountIncludeDiscount,
    decimal TotalDiscountAmount,
    List<CartResponse.CartItemResponse> Items)
{
    public record CartItemResponse(
        Guid Id,
        Guid ProductId,
        Guid? ProductVariantId,
        string ProductName,
        string? ProductAttributes,
        decimal ProductPrice,
        decimal? SpecialPrice,
        CartItemResponse.DiscountResponse? Discount,
        int Quantity,
        decimal SubTotal,
        decimal SubDiscountTotal)
    {
        public record DiscountResponse(
            bool UseDiscountPercentage,
            decimal Percentage,
            decimal Amount);
    }
};
