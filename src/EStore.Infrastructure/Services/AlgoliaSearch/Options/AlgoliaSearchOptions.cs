namespace EStore.Infrastructure.Services.AlgoliaSearch.Options;

public class AlgoliaSearchOptions
{
    public const string SectionName = "AlgoliaSearch";

    public string ApplicationId { get; set; } = null!;

    public string AdminApiKey { get; set; } = null!;

    public string SearchApiKey { get; set; } = null!;

    public string IndexName { get; set; } = null!;
}
