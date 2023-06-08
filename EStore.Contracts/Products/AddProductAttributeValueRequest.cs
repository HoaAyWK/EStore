namespace EStore.Contracts.Products;

public record AddProductAttributeValueRequest(
    string Name,
    string? Alias,
    decimal? PriceAdjustment);
