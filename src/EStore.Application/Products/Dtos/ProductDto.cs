namespace EStore.Application.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }

    public string ObjectID { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal? SpecialPrice { get; set; }

    public DateTime? SpecialPriceStartDate { get; set; }

    public DateTime? SpecialPriceEndDate { get; set; }

    public bool Published { get; set; }

    public int StockQuantity { get; set; }

    public AverageRatingDto AverageRating { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public BrandDto? Brand { get; set; }

    public CategoryDto? Category { get; set; }

    public DiscountDto? Discount { get; set; }

    public IEnumerable<ProductImageDto> Images { get; set; } = null!;

    public IEnumerable<ProductAttributeDto> Attributes { get; set; } = null!;

    public IEnumerable<ProductVariantDto> Variants { get; set; } = null!;
}
