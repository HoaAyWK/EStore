namespace EStore.Contracts.ProductAttributes;

public record UpdateProductAttributeRequest(
    string ProductAttributeId,
    string Name,
    string? Description,
    string? Alias);
