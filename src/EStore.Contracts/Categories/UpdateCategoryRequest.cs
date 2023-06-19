namespace EStore.Contracts.Categories;

public record UpdateCategoryRequest(string Name, string? ParentId);
