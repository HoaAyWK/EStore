namespace EStore.Contracts.Products;

public record UpdateProductVariantRequest(
    int StockQuantity,
    decimal? Price,
    bool IsActive,
    List<Guid>? AssignedImageIds);

