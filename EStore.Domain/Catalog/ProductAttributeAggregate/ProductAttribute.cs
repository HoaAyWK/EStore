using EStore.Domain.Common.Models;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAggregate;

namespace EStore.Domain.Catalog.ProductAttributeAggregate;

public sealed class ProductAttribute : AggregateRoot<ProductAttributeId>
{
    private List<ProductAttributeOptionSet> _productAttributeOptionSets = new();

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public string? Alias { get; private set; }

    public IReadOnlyList<ProductAttributeOptionSet> ProductAttributeOptionSets
        => _productAttributeOptionSets.AsReadOnly();

    private ProductAttribute()
    {
    }

    private ProductAttribute(
        ProductAttributeId id,
        string name,
        string? description,
        string? alias) : base(id)
    {
        Name = name;
        Description = description;
        Alias = alias;
    }

    public static ProductAttribute Create(
        string name,
        string? description = null,
        string? alias = null)
    {
        return new(
            ProductAttributeId.CreateUnique(),
            name,
            description,
            alias);
    }

    public void UpdateDetails(
        string name,
        string? description,
        string? alias)
    {
        Name = name;
        Description = description;
        Alias = alias;
    }

    public void AddOptionSet(ProductAttributeOptionSet optionSet)
    {
        _productAttributeOptionSets.Add(optionSet);
    }

    public void RemoveOptionSet(ProductAttributeOptionSet optionSet)
    {
        _productAttributeOptionSets.Remove(optionSet);
    }
}
