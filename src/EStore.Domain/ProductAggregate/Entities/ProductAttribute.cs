using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;
using ErrorOr;
using EStore.Domain.Common.Errors;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttribute : Entity<ProductAttributeId>
{
    private readonly List<ProductAttributeValue> _productAttributeValues = new();

    public string Name { get; private set; } = null!;

    public bool CanCombine { get; private set; }

    public bool Colorable { get; private set;}

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
        int displayOrder,
        bool colorable
        ) : base(id)
    {
        Name = name;
        CanCombine = canCombine;
        DisplayOrder = displayOrder;
        Colorable = colorable;
    }

    public static ProductAttribute Create(
        string name,
        bool canCombine,
        int displayOrder,
        bool colorable = false)
    {
        return new(
            ProductAttributeId.CreateUnique(),
            name,
            canCombine,
            displayOrder,
            colorable);
    }

    public ErrorOr<Updated> Update(string name, bool canCombine, int displayOrder, bool colorable)
    {
        if (Colorable != colorable && ProductAttributeValues.Any())
        {
            return Errors.Product.ProductAttributeAlreadyHadValues;
        }

        Name = name;
        CanCombine = canCombine;
        DisplayOrder = displayOrder;
        Colorable = colorable;

        return Result.Updated;
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
