using EStore.Application.Orders.Services;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.OrderSequenceManager;

public class OrderSequenceService : IOrderSequenceService
{
    private readonly EStoreDbContext _dbContext;

    public OrderSequenceService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> GetNextOrderNumberAsync()
    {
        var orderSequence = await _dbContext.OrderSequences.FirstOrDefaultAsync();

        if (orderSequence != null)
        {
            return orderSequence.LastOrderNumber + 1;
        }

        orderSequence = new OrderSequence(1);

        await _dbContext.OrderSequences.AddAsync(orderSequence);
        await _dbContext.SaveChangesAsync();

        return orderSequence.LastOrderNumber + 1;
    }

    public async Task IncreaseLastOrderNumberAsync()
    {
        var orderSequence = await _dbContext.OrderSequences.FirstOrDefaultAsync();

        if (orderSequence != null)
        {
            orderSequence.LastOrderNumber++;
        }
        else
        {
            orderSequence = new OrderSequence(1);
            await _dbContext.OrderSequences.AddAsync(orderSequence);
        }

        await _dbContext.SaveChangesAsync();
    }
}
