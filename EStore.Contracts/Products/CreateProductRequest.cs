namespace EStore.Contracts.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    bool Published,
    decimal Price,
    string BrandId,
    string CategoryId);
