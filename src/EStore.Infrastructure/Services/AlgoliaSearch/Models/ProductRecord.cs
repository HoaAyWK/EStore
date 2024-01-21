namespace EStore.Infrastructure.Services.AlgoliaSearch.Models;

public class ProductRecord
{
    public string ObjectID { get; set; } = string.Empty;

    public Guid ProductVariantId { get; set; }

    public Guid ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal? SpecialPrice { get; set; }

    public DateTime? SpecialPriceStartDateTime { get; set; }

    public DateTime? SpecialPriceEndDateTime { get; set; }

    public int StockQuantity { get; set; }

    public double AverageRating { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string? Brand { get; set; }

    public bool IsActive { get; set; }

    public List<string> Categories { get; set; } = new();

    public DiscountRecord? Discount { get; set; }

    public string Image { get; set; } = null!;
}

public class DiscountRecord
{
    public bool UsePercentage { get; set; }

    public decimal Percentage { get; set; }

    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}

