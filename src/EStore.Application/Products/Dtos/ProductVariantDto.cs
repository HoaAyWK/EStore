namespace EStore.Application.Products.Dtos;

public class ProductVariantDto
{
    public Guid Id { get; set; }

    public decimal? Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }

    public string AssignedProductImageIds { get; set; } = string.Empty;

    public string? RawAttributeSelection { get; set; }
}
