namespace EStore.Contracts.Categories;

public class CategoryWithPathResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string Path { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public bool HasChild { get; set; }
}
