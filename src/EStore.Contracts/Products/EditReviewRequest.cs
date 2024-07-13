namespace EStore.Contracts.Products;

public record EditProductReviewRequest(
    Guid? ProductVariantId,
    string Content,
    int Rating);
