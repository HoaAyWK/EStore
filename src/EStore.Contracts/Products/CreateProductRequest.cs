namespace EStore.Contracts.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    bool Published,
    decimal Price,
    int DisplayOrder,
    Guid BrandId,
    Guid CategoryId,
    bool HasVariant);
