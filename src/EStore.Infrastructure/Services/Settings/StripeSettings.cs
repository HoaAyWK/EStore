namespace EStore.Infrastructure.Services.Settings;

public sealed class StripeSettings
{
    public const string SectionName = "StripeSettings";

    public string SecretKey { get; init; } = string.Empty;

    public string PublishKey { get; init; } = string.Empty;

    public string SuccessUrl { get; init; } = string.Empty;

    public string CancelUrl { get; init; } = string.Empty;
}
