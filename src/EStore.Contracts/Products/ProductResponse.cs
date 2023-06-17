namespace EStore.Contracts.Products;

public class ProductResponse
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? SpecialPrice { get; set; }

    public DateTime? SpecialPriceStartDate { get; set; }

    public DateTime? SpecialPriceEndDate { get; set; }

    public bool Published { get; set; }

    public AverageRatingResponse? AverageRating { get; set; }

    public BrandResponse? Brand { get; set; }

    public CategoryResponse? Category { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public List<ProductImageResponse>? Images { get; set; }

    public List<ProductAttributeResponse>? Attributes { get; set; }

    public List<ProductVariantResponse>? Variants { get; set; }
}

public record BrandResponse(string Id, string Name);

public record CategoryResponse(string Id, string Name);

public record AverageRatingResponse(
    double Value,
    int NumRatings);

public class ProductAttributeResponse
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<ProductAttributeValueResponse>? AttributeValues { get; set; }
}

public record ProductImageResponse(
    Guid Id,
    string ImageUrl,
    bool IsMain,
    int DisplayOrder);

public record ProductAttributeValueResponse(
    string Id,
    string Name,
    string? Alias,
    decimal? PriceAdjustment,
    Dictionary<Guid, List<Guid>> CombinedAttributes);

public record ProductVariantResponse(
    string Id,
    decimal? Price,
    int StockQuantity,
    bool IsActive,
    List<string>? AssignedProductImageIds);
