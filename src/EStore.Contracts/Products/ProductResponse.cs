namespace EStore.Contracts.Products;

public class ProductResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool Published { get; set; }

    public int DisplayOrder { get; set; }

    public int StockQuantity { get; set; }

    public AverageRatingResponse? AverageRating { get; set; }

    public BrandResponse? Brand { get; set; }

    public CategoryResponse? Category { get; set; }

    public DiscountResponse? Discount { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public bool HasVariant { get; set; }

    public List<ProductImageResponse> Images { get; set; } = new();

    public List<ProductAttributeResponse> Attributes { get; set; } = new();

    public List<ProductVariantResponse> Variants { get; set; } = new();

    public List<ProductReviewResponse> Reviews { get; set; } = new();
}

public record BrandResponse(Guid Id, string Name);

public record CategoryResponse(Guid Id, string Name, string Slug);

public record AverageRatingResponse(
    double Value,
    int NumRatings);

public record ProductAttributeResponse(
    Guid Id,
    string Name,
    bool CanCombine,
    int DisplayOrder,
    bool Colorable,
    List<ProductAttributeValueResponse> AttributeValues);

public record ProductImageResponse(
    Guid Id,
    string ImageUrl,
    bool IsMain,
    int DisplayOrder);

public record ProductAttributeValueResponse(
    Guid Id,
    string Name,
    string? Color,
    decimal? PriceAdjustment,
    string? RawCombinedAttribute,
    int DisplayOrder,
    Dictionary<Guid, List<Guid>> CombinedAttributes);

public record ProductVariantResponse(
    Guid Id,
    decimal? Price,
    int StockQuantity,
    bool IsActive,
    AverageRatingResponse AverageRating,
    List<string>? AssignedProductImageIds,
    Dictionary<string, string> Attributes,
    Dictionary<Guid, Guid> AttributeSelection);

public record DiscountResponse(
    Guid Id,
    string Name,
    bool UsePercentage,
    decimal Percentage,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate);

public record ProductReviewResponse(
    Guid Id,
    string Content,
    int Rating,
    ProductReviewOwnerResponse? Owner,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime,
    Dictionary<Guid, Guid> AttributeSelection,
    Dictionary<string, string> Attributes,
    List<ProductReviewCommentResponse> ReviewComments);

public record ProductReviewCommentResponse(
    Guid Id,
    string Content,
    ProductReviewOwnerResponse? Owner,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);

public record ProductReviewOwnerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? AvatarUrl);
