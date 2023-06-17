namespace EStore.Contracts.Categories;

public record CategoryNodeResponse(
    Guid Id,
    string Name,
    Guid? ParentId,
    int Level,
    List<CategoryNodeResponse> Children);
