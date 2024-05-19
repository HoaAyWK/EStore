namespace EStore.Contracts.Orders;

public class OrderStats
{
    public int TotalOrders { get; set; }

    public List<int> OrdersPerDay { get; set; } = new();

    public int FromDay { get; set; }

    public double CompareToPreviousDays { get; set; }

    public bool IsIncreased { get; set; }
}
