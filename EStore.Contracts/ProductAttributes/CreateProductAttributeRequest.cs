namespace EStore.Contracts.ProductAttributes;

public record CreateProductAttributeRequest(
    string Name,
    string? Description,
    string? Alias);
