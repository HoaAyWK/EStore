namespace EStore.Contracts.Products;

public record UpdateProductAttributeValueRequest(
    string Name,
    string? Color,
    decimal? PriceAdjustment,
    int DisplayOrder);
