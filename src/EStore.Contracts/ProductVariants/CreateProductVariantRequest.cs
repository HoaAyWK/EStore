namespace EStore.Contracts.ProductVariants;

public record CreateProductVariantRequest(
    Guid ProductId,
    int StockQuantity,
    bool IsActive,
    List<CreateProductVariantRequest.SelectedAttribute> SelectedAttributes,
    List<Guid>? AssignedImageIds)
{
    public record SelectedAttribute(
        Guid ProductAttributeId,
        Guid ProductAttributeValueId);
}

