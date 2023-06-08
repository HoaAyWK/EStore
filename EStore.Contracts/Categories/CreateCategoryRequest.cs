namespace EStore.Contracts.Categories;

public record CreateCategoryRequest(
    string Name,
    string? ParentId);
