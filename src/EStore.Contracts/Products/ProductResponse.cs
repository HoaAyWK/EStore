namespace EStore.Contracts.Products;

public class ProductResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal? SpecialPrice { get; set; }

    public DateTime? SpecialPriceStartDate { get; set; }

    public DateTime? SpecialPriceEndDate { get; set; }

    public bool Published { get; set; }

    public int DisplayOrder { get; set; }

    public int StockQuantity { get; set; }

    public AverageRatingResponse? AverageRating { get; set; }

    public BrandResponse? Brand { get; set; }

    public CategoryResponse? Category { get; set; }

    public DiscountResponse? Discount { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public List<ProductImageResponse> Images { get; set; } = new();

    public List<ProductAttributeResponse> Attributes { get; set; } = new();

    public List<ProductVariantResponse> Variants { get; set; } = new();

}

public record BrandResponse(Guid Id, string Name);

public record CategoryResponse(Guid Id, string Name);

public record AverageRatingResponse(
    double Value,
    int NumRatings);

public record ProductAttributeResponse(
    Guid Id,
    string Name,
    List<ProductAttributeValueResponse> AttributeValues);

public record ProductImageResponse(
    Guid Id,
    string ImageUrl,
    bool IsMain,
    int DisplayOrder);

public record ProductAttributeValueResponse(
    Guid Id,
    string Name,
    string? Alias,
    decimal? PriceAdjustment,
    string? RawCombinedAttribute,
    Dictionary<Guid, List<Guid>> CombinedAttributes);

public record ProductVariantResponse(
    Guid Id,
    decimal? Price,
    int StockQuantity,
    bool IsActive,
    List<string>? AssignedProductImageIds);

public record DiscountResponse(
    Guid Id,
    string Name,
    bool UsePercentage,
    decimal Percentage,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate);
