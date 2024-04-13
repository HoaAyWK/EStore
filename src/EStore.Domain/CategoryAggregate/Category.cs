using ErrorOr;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;

namespace EStore.Domain.CategoryAggregate;

public sealed class Category : AggregateRoot<CategoryId>, IAuditableEntity, ISoftDeletableEntity
{
    public const int MinNameLength = 2;

    public const int MaxNameLength = 100;

    public const int MinSlugLength = 2;

    public const int MaxSlugLength = 150;

    private readonly List<Category> _children = new();

    public string Name { get; private set; } = null!;

    public CategoryId? ParentId { get; private set; }

    public Category? Parent { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    public string Slug { get; private set; } = null!;

    public string? ImageUrl { get; private set; }

    public IReadOnlyList<Category> Children => _children.AsReadOnly();

    private Category()
    {
    }

    private Category(
        CategoryId categoryId,
        string name,
        string slug,
        string? imageUrl,
        CategoryId? parentId) : base(categoryId)
    {
        Name = name;
        Slug = slug.ToLowerInvariant();
        ImageUrl = imageUrl;
        ParentId = parentId;
    }

    public static ErrorOr<Category> Create(
        string name,
        string slug,
        string? imageUrl,
        CategoryId? parentId)
    {
        var errors = ValidateName(name);

        errors.AddRange(ValidateSlug(slug));

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Category(
            CategoryId.CreateUnique(),
            name,
            slug,
            imageUrl,
            parentId);
    }

    public ErrorOr<Updated> UpdateDetails(
        string name,
        string? imageUrl,
        string slug)
    {
        var errors = ValidateName(name);

        errors.AddRange(ValidateSlug(slug));

        if (errors.Count > 0)
        {
            return errors;
        }

        Name = name;
        Slug = slug.ToLowerInvariant();
        ImageUrl = imageUrl;

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

    private static List<Error> ValidateSlug(string slug)
    {
        List<Error> errors = new();

        if (slug.Length is < MinSlugLength or > MaxSlugLength)
        {
            errors.Add(Errors.Category.InvalidSlugLength);
        }

        return errors;
    }
}
