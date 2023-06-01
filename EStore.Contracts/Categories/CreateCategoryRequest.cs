namespace EStore.Contracts.Categories;

public record CreateCategoryRequest(
    string Name,
    Guid? ParentId);
