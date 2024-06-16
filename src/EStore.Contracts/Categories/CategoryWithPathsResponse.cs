namespace EStore.Contracts.Categories;

public class CategoryWithPathsResponse
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? ImageUrl { get; set; }

    public Guid? ParentId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public Dictionary<string, string> Paths { get; set; } = new();
}
