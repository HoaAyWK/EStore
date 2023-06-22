namespace EStore.Contracts.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    bool Published,
    int DisplayOrder,
    Guid BrandId,
    Guid CategoryId);
