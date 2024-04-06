namespace EStore.Contracts.Products;

public record AddProductAttributeValueRequest(
    string Name,
    string? Color,
    decimal? PriceAdjustment,
    int DisplayOrder);
