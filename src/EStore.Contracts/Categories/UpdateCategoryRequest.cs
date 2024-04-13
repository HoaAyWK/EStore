namespace EStore.Contracts.Categories;

public record UpdateCategoryRequest(
    string Name,
    string Slug,
    string? ImageUrl,
    string? ParentId);
