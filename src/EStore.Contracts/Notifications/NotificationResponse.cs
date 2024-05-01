namespace EStore.Contracts.Notifications;

public class NotificationResponse
{
    public Guid Id { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public FromResponse From { get; set; } = null!;

    public Guid To { get; set; }

    public bool IsRead { get; set; }

    public DateTime Timestamp { get; set; }
}

public record FromResponse(
    Guid Id,
    string FullName,
    string Email,
    string? AvatarUrl);
