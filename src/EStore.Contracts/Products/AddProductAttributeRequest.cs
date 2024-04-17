namespace EStore.Contracts.Products;

public record AddProductAttributeRequest(
    string Name,
    string? Alias,
    bool CanCombine,
    bool Colorable,
    int DisplayOrder);
