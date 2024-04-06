namespace EStore.Contracts.Products;

public record AddProductAttributeRequest(
    string Name,
    string? Alias,
    bool CanCombine,
    int DisplayOrder);
