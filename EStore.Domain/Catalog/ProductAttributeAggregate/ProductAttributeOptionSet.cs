using EStore.Domain.Common.Models;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAttributeAggregate;

public sealed class ProductAttributeOptionSet : Entity<ProductAttributeOptionSetId>
{
    private List<ProductAttributeOption> _productAttributeOptions = new();

    public string Name { get; private set; } = null!;

    public IReadOnlyList<ProductAttributeOption> ProductAttributeOptions
        => _productAttributeOptions.AsReadOnly();

    private ProductAttributeOptionSet()
    {
    }

    private ProductAttributeOptionSet(
        ProductAttributeOptionSetId id,
        string name)
        : base(id)
    {
        Name = name;
    }

    public static ProductAttributeOptionSet Create(string name)
    {
        return new(ProductAttributeOptionSetId.CreateUnique(), name);
    }

    public void AddOption(ProductAttributeOption option)
    {
        _productAttributeOptions.Add(option);
    }

    public void RemoveOption(ProductAttributeOption option)
    {
        _productAttributeOptions.Remove(option);
    }
}
