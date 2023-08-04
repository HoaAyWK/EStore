namespace EStore.Application.Products.Dtos;

public class DiscountDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool UsePercentage { get; set; }

    public decimal Percentage { get; set; }

    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
