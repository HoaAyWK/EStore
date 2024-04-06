namespace EStore.Domain.Common.Abstractions;

public interface ISoftDeletableEntity
{
    DateTime? DeletedOnUtc { get; }

    bool Deleted { get; }
}
