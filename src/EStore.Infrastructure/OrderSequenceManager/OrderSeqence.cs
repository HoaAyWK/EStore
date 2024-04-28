namespace EStore.Infrastructure.OrderSequenceManager;

public class OrderSequence
{
    public int Id { get; set; }

    public long LastOrderNumber { get; set; }

    private OrderSequence()
    {
    }

    public OrderSequence(long lastOrderNumber)
    {
        LastOrderNumber = lastOrderNumber;
    }
}
