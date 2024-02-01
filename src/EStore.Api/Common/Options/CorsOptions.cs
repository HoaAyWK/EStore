namespace EStore.Api.Common.Options;

public class CorsOptions
{
    public const string SectionName = "CorsSettings";

    public string PolicyName { get; set; } = null!;

    public string AllowedOrigins { get; set; } = null!;

    public string AllowedMethods { get; set; } = null!;

    public string AllowedHeaders { get; set; } = null!;
}
