namespace EStore.Contracts.Products;

public record UpdateVariantRequest(
    int StockQuantity,
    decimal Price,
    bool IsActive,
    List<string>? AssignedImageIds);
