using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttribute : Entity<ProductAttributeId>
{
    private readonly List<ProductAttributeValue> _productAttributeValues = new();

    public string Name { get; private set; } = null!;

    public string? Alias { get; private set; }

    public bool CanCombine { get; private set; }

    public IReadOnlyList<ProductAttributeValue> ProductAttributeValues
        => _productAttributeValues.AsReadOnly();

    private ProductAttribute()
    {
    }

    private ProductAttribute(
        ProductAttributeId id,
        string name,
        string? alias,
        bool canCombine
        ) : base(id)
    {
        Name = name;
        Alias = alias;
        CanCombine = canCombine;
    }

    public static ProductAttribute Create(
        string name,
        string? alias,
        bool canCombine)
    {
        return new(
            ProductAttributeId.CreateUnique(),
            name,
            alias,
            canCombine);
    }

    public void Update(string name, string? alias, bool canCombine)
    {
        Name = name;
        Alias = alias;
        CanCombine = canCombine;
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
