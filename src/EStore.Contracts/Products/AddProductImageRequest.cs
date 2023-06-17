namespace EStore.Contracts.Products;

public record AddProductImageRequest(
    string ImageUrl,
    bool? IsMain,
    int DisplayOrder);
