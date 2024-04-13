namespace EStore.Application.Products.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }

    public string Slug { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string Name { get; set; } = string.Empty;
}