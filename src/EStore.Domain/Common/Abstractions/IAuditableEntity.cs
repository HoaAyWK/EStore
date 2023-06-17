namespace EStore.Domain.Common.Abstractions;

public interface IAuditableEntity
{
    DateTime CreatedDateTime { get; }

    DateTime UpdatedDateTime { get; }
}
