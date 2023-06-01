namespace EStore.Application.Products.Common;

public record CreateProductImageRequest(
    string ImageUrl,
    int DisplayOrder);
