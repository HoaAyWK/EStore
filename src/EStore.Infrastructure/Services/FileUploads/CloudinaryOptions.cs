namespace EStore.Infrastructure.Services.FileUploads;

public class CloudinaryOptions
{
    public const string SectionName = "Cloudinary";

    public string CloudName { get; set; } = default!;

    public string ApiKey { get; set; } = default!;

    public string ApiSecret { get; set; }  = default!;
}
