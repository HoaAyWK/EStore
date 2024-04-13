namespace EStore.Contracts.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    string Slug,
    string? ImageUrl,
    Guid? ParentId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
