namespace EStore.Contracts.Categories;

public record CreateCategoryRequest(
    string Name,
    string Slug,
    string? ImageUrl,
    string? ParentId);
