namespace EStore.Contracts.Products;

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    bool Published,
    Guid BrandId,
    Guid CategoryId,
    decimal? SpecialPrice,
    DateTime? SpecialPriceStartDate,
    DateTime? SpecialPriceEndDate);
