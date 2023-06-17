namespace EStore.Contracts.Categories;

public record UpdateCategoryRequest(Guid Id, string Name, string? ParentId);
