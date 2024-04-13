using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Dtos;

public class ProductVariantDto
{
    public Guid Id { get; set; }

    public decimal? Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }

    public string AssignedProductImageIds { get; set; } = string.Empty;

    public string? RawAttributeSelection { get; set; }

    public string RawAttributes { get; set; } = string.Empty;

    public AverageRatingDto AverageRating { get; set; } = null!;

    public Dictionary<string, string> Attributes
        => JsonConvert.DeserializeObject<Dictionary<string, string>>(RawAttributes) ?? new();

    public AttributeSelection<ProductAttributeId, ProductAttributeValueId> AttributeSelection
        => AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(RawAttributeSelection);
}
