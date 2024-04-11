using System.Dynamic;

namespace EStore.Contracts.Searching;

public class ProductSearchModel
{
    public string ObjectID { get; set; } = default!;

    public Guid ProductId { get; set; }

    public Guid ProductVariantId { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public dynamic Categories { get; set; } = new ExpandoObject();

    public bool HasVariant { get; set; }

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public decimal FinalPrice { get; set; }

    public string? Image { get; set; }

    public double AverageRating { get; set; }

    public int NumRatings { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public ProductSearchDiscount? Discount { get; set; }

    public Dictionary<string, string> Attributes { get; set; } = new();
}

public class ProductSearchDiscount
{
    public bool UsePercentage { get; set; }

    public decimal DiscountPercentage { get; set; }

    public decimal DiscountAmount { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }
}
