namespace EStore.Contracts.Products;

public record UpdateProductRequest(
    string Name,
    string ShortDescription,
    string Description,
    decimal Price,
    bool Published,
    int DisplayOrder,
    Guid BrandId,
    Guid CategoryId,
    Guid? DiscountId,
    int? StockQuantity,
    bool HasVariant);
