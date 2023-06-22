namespace EStore.Contracts.ProductVariants;

public record UpdateProductVariantRequest(
    Guid Id,
    int StockQuantity,
    decimal? Price,
    bool IsActive,
    List<Guid>? AssignedImageIds);
