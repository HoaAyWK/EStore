namespace EStore.Contracts.Products;

public record UpdateProductAttributeRequest(
    Guid Id,
    Guid ProductId,
    string Name,
    bool CanCombine,
    int DisplayOrder,
    bool Colorable);
