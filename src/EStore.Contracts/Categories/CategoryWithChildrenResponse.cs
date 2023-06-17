namespace EStore.Contracts.Categories;

public record CategoryWithChildrenResponse(
    Guid Id,
    string Name,
    string? ParentId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime,
    List<CategoryWithChildrenResponse> Children);
