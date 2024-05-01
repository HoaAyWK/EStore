using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate.ValueObjects;

namespace EStore.Domain.NotificationAggregate;

public sealed class Notification : AggregateRoot<NotificationId>
{
    public string Message { get; private set; } = string.Empty;

    public string Domain { get; private set; } = string.Empty;

    public string Type { get; private set; } = string.Empty;

    public Guid EntityId { get; private set; }

    public CustomerId From { get; private set; } = null!;

    public CustomerId To { get; private set; } = null!;

    public bool IsRead { get; private set; }

    public DateTime Timestamp { get; private set; }

    private Notification()
    {
    }

    private Notification(
        NotificationId id,
        string message,
        string domain,
        string type,
        Guid entityId,
        CustomerId from,
        CustomerId to,
        bool isRead,
        DateTime timestamp)
        : base(id)
    {
        Message = message;
        Domain = domain;
        Type = type;
        EntityId = entityId;
        From = from;
        To = to;
        IsRead = isRead;
        Timestamp = timestamp;
    }

    public static Notification Create(
        string message,
        string domain,
        string type,
        Guid entityId,
        CustomerId from,
        CustomerId to,
        bool isRead,
        DateTime timestamp)
    {
        return new Notification(
            NotificationId.CreateUnique(),
            message,
            domain,
            type,
            entityId,
            from,
            to,
            isRead,
            timestamp);
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
