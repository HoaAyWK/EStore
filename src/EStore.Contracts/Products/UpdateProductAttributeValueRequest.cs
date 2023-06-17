namespace EStore.Contracts.Products;

public record UpdateProductAttributeValueRequest(
    string Name,
    string? Alias,
    decimal? PriceAdjustment);
