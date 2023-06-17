namespace EStore.Contracts.Products;

public record UpdateProductAttributeRequest(
    Guid Id,
    Guid ProductId,
    string Name,
    string? Alias,
    bool CanCombine);
