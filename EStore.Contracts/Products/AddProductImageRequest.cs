namespace EStore.Contracts.Products;

public record AddProductImageRequest(
    string ProductId,
    string ImageUrl,
    bool? IsMain,
    int DisplayOrder);
