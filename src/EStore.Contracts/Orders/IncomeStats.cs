namespace EStore.Contracts.Orders;

public class IncomeStats
{
    public decimal TotalIncome { get; set; }

    public Dictionary<DateTime, decimal> IncomePerDay { get; set; } = new();

    public int FromDay { get; set; }

    public decimal CompareToPreviousDays { get; set; }

    public bool IsIncreased { get; set; }
}
