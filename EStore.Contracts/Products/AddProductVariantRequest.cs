namespace EStore.Contracts.Products;

public class AddProductVariantRequest
{
    public List<SelectedAttributes> AttributeSelections { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool? IsActive { get; set; }

    public List<string>? AssignedImageIds { get; set; }
}

public record SelectedAttributes(
    Guid ProductAttributeId,
    Guid ProductAttributeValueId);
