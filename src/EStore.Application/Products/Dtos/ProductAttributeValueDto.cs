using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Dtos;

public class ProductAttributeValueDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Color { get; set; }

    public decimal PriceAdjustment { get; set; }

    public string? RawCombinedAttributes { get; set; }

    public AttributeSelection<ProductAttributeId, ProductAttributeValueId> AttributeSelection
        => AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(RawCombinedAttributes);
}
