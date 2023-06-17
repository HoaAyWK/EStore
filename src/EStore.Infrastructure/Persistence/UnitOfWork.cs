using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        UpdateAuditableEntities();
        
        return _dbContext.SaveChangesAsync(cancellationToken);
    }


    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries = 
            _dbContext.ChangeTracker.Entries<IAuditableEntity>();
        
        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.CreatedDateTime).CurrentValue = DateTime.UtcNow;
                entityEntry.Property(e => e.UpdatedDateTime).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(e => e.UpdatedDateTime).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
