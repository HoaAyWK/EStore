namespace EStore.Contracts.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    string? ParentId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
