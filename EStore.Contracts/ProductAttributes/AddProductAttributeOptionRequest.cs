namespace EStore.Contracts.ProductAttributes;

public class AddProductAttributeOptionRequest
{
    public string ProductAttributeId { get; set; } = null!;

    public string ProductAttributeOptionSetId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal? PriceAdjustment { get; set; }

    public string? Alias { get; set; }
}