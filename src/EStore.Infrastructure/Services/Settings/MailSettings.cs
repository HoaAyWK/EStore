namespace EStore.Infrastructure.Services.Settings;

public class MailSettings
{
    public const string SectionName = "Mail";

    public string SenderEmail { get; init; } = null!;

    public string SenderDisplayName { get; init; } = null!;

    public string SmtpServer { get; init; } = null!;

    public string SmtpPassword { get; init; } = null!;

    public int SmtpPort { get; init; }
}
