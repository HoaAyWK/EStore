namespace EStore.Contracts.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    bool Published,
    Guid BrandId,
    Guid CategoryId);
