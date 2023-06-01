namespace EStore.Contracts.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    bool Published,
    string BrandId,
    string CategoryId);
