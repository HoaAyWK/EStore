using EStore.Application.Common.Interfaces.Persistence;

namespace EStore.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EStoreDbContext _dbContext;

    public UnitOfWork(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
