namespace EStore.Contracts.Products;

public record UpdateProductVariantRequest(
    Guid Id,
    int StockQuantity,
    decimal? Price,
    bool IsActive,
    List<Guid>? AssignedImageIds);

