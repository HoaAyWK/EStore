namespace EStore.Contracts.Categories;

public record CategoryNodeResponse(
    Guid Id,
    string Name,
    string Slug,
    string? ImageUrl,
    Guid? ParentId,
    int Level,
    List<CategoryNodeResponse> Children);
