namespace EStore.Contracts.Customers;

public class CustomerStats
{
     public int TotalCustomers { get; set; }

    public List<int> CustomerRegistersPerDay { get; set; } = new();

    public int FromDay { get; set; }

    public double CompareToPreviousDays { get; set; }

    public bool IsIncreased { get; set; }
}
