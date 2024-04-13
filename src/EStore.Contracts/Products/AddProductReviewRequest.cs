namespace EStore.Contracts.Products;

public record AddProductReviewRequest(
    Guid ProductId,
    Guid? ProductVariantId,
    Guid OwnerId,
    string Content,
    int Rating);
