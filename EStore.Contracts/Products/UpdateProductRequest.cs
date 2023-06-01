namespace EStore.Contracts.Products;

public record UpdateProductRequest(
    string ProductId,
    string Name,
    string Description,
    decimal Price,
    bool Published,
    string BrandId,
    string CategoryId,
    decimal? SpecialPrice,
    DateTime? SpecialPriceStartDate,
    DateTime? SpecialPriceEndDate);
