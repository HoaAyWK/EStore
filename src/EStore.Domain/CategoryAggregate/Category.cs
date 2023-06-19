using ErrorOr;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;

namespace EStore.Domain.CategoryAggregate;

public sealed class Category : AggregateRoot<CategoryId>, IAuditableEntity
{
    public const int MinNameLength = 2;

    public const int MaxNameLength = 100;

    private readonly List<Category> _children = new();

    public string Name { get; private set; } = null!;

    public CategoryId? ParentId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public Category? Parent { get; private set; }

    public IReadOnlyList<Category> Children => _children.AsReadOnly();

    private Category()
    {
    }

    private Category(
        CategoryId categoryId,
        string name,
        CategoryId? parentId) : base(categoryId)
    {
        Name = name;
        ParentId = parentId;
    }

    public static ErrorOr<Category> Create(string name, CategoryId? parentId)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Category(
            CategoryId.CreateUnique(),
            name,
            parentId);
    }

    public ErrorOr<Updated> UpdateName(string name)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        Name = name;

        return Result.Updated;
    }

    public void UpdateParentCategory(CategoryId? parentId)
    {
        ParentId = parentId;
    }

    public void AddChildren(Category child)
    {
        _children.Add(child);
    }

    private static List<Error> ValidateName(string name)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Category.InvalidNameLength);
        }

        return errors;
    }
}
