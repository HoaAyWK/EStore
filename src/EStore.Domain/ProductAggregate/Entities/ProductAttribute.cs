using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;
using ErrorOr;
using EStore.Domain.Common.Errors;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttribute : Entity<ProductAttributeId>
{
    public const int MinNameLength = 1;

    public const int MaxNameLength = 100;

    private readonly List<ProductAttributeValue> _productAttributeValues = new();

    public string Name { get; private set; } = null!;

    public bool CanCombine { get; private set; }

    public bool Colorable { get; private set;}

    public int DisplayOrder { get; private set; }

    public IReadOnlyList<ProductAttributeValue> ProductAttributeValues
        => _productAttributeValues.AsReadOnly();

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

    public static ErrorOr<ProductAttribute> Create(
        string name,
        bool canCombine,
        int displayOrder,
        bool colorable = false)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new ProductAttribute(
            ProductAttributeId.CreateUnique(),
            name,
            canCombine,
            displayOrder,
            colorable);
    }

    public ErrorOr<Updated> Update(
        string name,
        bool canCombine,
        int displayOrder,
        bool colorable)
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

    private static List<Error> ValidateName(string name)
    {
        var errors = new List<Error>();

        if (name.Length < MinNameLength || name.Length > MaxNameLength)
        {
            errors.Add(Errors.Product.InvalidProductAttributeNameLength);
        }

        return errors;
    }
}
