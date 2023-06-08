using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.CategoryAggregate;

public sealed class Category : AggregateRoot<CategoryId>
{
    private readonly List<Category> _children = new();

    public string Name { get; private set; } = null!;

    public CategoryId? ParentId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public IReadOnlyList<Category> Children => _children.AsReadOnly();

    public Category? Parent { get; private set; }

    private Category()
    {
    }

    private Category(
        CategoryId categoryId,
        string name,
        CategoryId? parentId,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(categoryId)
    {
        Name = name;
        ParentId = parentId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Category Create(string name, CategoryId? parentId)
    {
        return new(
            CategoryId.CreateUnique(),
            name,
            parentId,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateParentCategory(CategoryId parentId)
    {
        ParentId = parentId;
    }

    public void AddChildCategory(Category category)
    {
        _children.Add(category);
    }

}
