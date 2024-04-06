using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EStore.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EStoreDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UnitOfWork(EStoreDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DateTime utcNow = _dateTimeProvider.UtcNow;

        UpdateAuditableEntities(utcNow);
        UpdateSoftDeletableEntities(utcNow);
        
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries = 
            _dbContext.ChangeTracker.Entries<IAuditableEntity>();
        
        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.CreatedDateTime).CurrentValue = utcNow;
                entityEntry.Property(e => e.UpdatedDateTime).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(e => e.UpdatedDateTime).CurrentValue = utcNow;
            }
        }
    }

    private void UpdateSoftDeletableEntities(DateTime utcNow)
    {
        IEnumerable<EntityEntry<ISoftDeletableEntity>> entries = 
            _dbContext.ChangeTracker.Entries<ISoftDeletableEntity>();

        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in entries)
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = utcNow;

            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (ReferenceEntry referenceEntry in
            entityEntry.References.Where(r => r.TargetEntry != null && r.TargetEntry.State == EntityState.Deleted))
        {
            if (referenceEntry.TargetEntry is not null)
            {
                referenceEntry.TargetEntry.State = EntityState.Unchanged;

                UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
            }
        }
    }
}
