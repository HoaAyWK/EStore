namespace EStore.Contracts.Searching;

public class SearchProductListPagedResponse
{
    public List<SearchProductResponse> Hits { get; set; } = new();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalHits { get; set; }

    public int TotalPages { get; set; }
}