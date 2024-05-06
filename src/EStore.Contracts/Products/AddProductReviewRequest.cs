namespace EStore.Contracts.Products;

public record AddProductReviewRequest(
    Guid ProductId,
    Guid? ProductVariantId,
    string Content,
    int Rating);
