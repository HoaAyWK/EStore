namespace EStore.Contracts.Products;

public record AddProductReviewRequest(
    Guid ProductId,
    Guid? ProductVariantId,
    Guid OwnerId,
    string Title,
    string Content,
    int Rating);
