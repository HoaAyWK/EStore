namespace EStore.Contracts.Searching;

public class ProductSearchModel
{
    public string ObjectID { get; set; } = default!;

    public Guid ProductId { get; set; }

    public Guid ProductVariantId { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public List<string> Categories { get; set; } = new();

    public decimal? SpecialPrice { get; set; }

    public DateTime? SpecialPriceStartDateTime { get; set; }

    public DateTime? SpecialPriceEndDateTime { get; set; }

    public bool HasVariant { get; set; }

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public double AverageRating { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public Dictionary<string, string> Attributes { get; set; } = new();
}
