namespace EStore.Contracts.Searching;

public class SearchProductResponse
{
    public string ObjectID { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public List<string> Categories { get; set; } = new();

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public double AverageRating { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }
}
