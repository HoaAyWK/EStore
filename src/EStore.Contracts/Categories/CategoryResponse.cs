namespace EStore.Contracts.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    Guid? ParentId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
