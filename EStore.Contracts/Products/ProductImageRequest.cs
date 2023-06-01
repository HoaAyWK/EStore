namespace EStore.Contracts.Products;

public record ProductImageRequest(
    string ImageUrl,
    int DisplayOrder);
