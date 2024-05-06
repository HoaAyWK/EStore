using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Products.Dtos;

public class ProductReviewDto
{
    public Guid Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public int Rating { get; set; }

    public ProductReviewOwnerDto? Owner { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string? RawAttributeSelection { get; set; }

    public string RawAttributes { get; set; } = string.Empty;

    public IEnumerable<ProductReviewCommentDto> ReviewComments { get; set; } = null!;

    public Dictionary<string, string> Attributes
        => JsonConvert.DeserializeObject<Dictionary<string, string>>(RawAttributes) ?? new();

    public AttributeSelection<ProductAttributeId, ProductAttributeValueId> AttributeSelection
        => AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(RawAttributeSelection);
}
