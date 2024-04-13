namespace EStore.Contracts.Categories;

public record CategoryWithChildrenResponse(
    Guid Id,
    string Name,
    string Slug,
    string? ImageUrl,
    string? ParentId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime,
    List<CategoryWithChildrenResponse> Children);
