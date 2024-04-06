using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttribute : Entity<ProductAttributeId>
{
    private readonly List<ProductAttributeValue> _productAttributeValues = new();

    public string Name { get; private set; } = null!;

    public bool CanCombine { get; private set; }

    public int DisplayOrder { get; private set; }

    public IReadOnlyList<ProductAttributeValue> ProductAttributeValues
        => _productAttributeValues.AsReadOnly();

    private ProductAttribute()
    {
    }

    private ProductAttribute(
        ProductAttributeId id,
        string name,
        bool canCombine,
        int displayOrder
        ) : base(id)
    {
        Name = name;
        CanCombine = canCombine;
        DisplayOrder = displayOrder;
    }

    public static ProductAttribute Create(
        string name,
        bool canCombine,
        int displayOrder)
    {
        return new(
            ProductAttributeId.CreateUnique(),
            name,
            canCombine,
            displayOrder);
    }

    public void Update(string name, bool canCombine, int displayOrder)
    {
        Name = name;
        CanCombine = canCombine;
        DisplayOrder = displayOrder;
    }

    public void AddAttributeValue(ProductAttributeValue attributeValue)
    {
        _productAttributeValues.Add(attributeValue);
    }

    public void RemoveAttributeValue(ProductAttributeValue attributeValue)
    {
        _productAttributeValues.Remove(attributeValue);
    }
}
