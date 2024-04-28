namespace EStore.Application.Orders.Services;

public interface IOrderSequenceService
{
    Task<long> GetNextOrderNumberAsync();
    Task IncreaseLastOrderNumberAsync();
}
