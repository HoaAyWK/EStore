namespace EStore.Contracts.Common;

public class PagedList<T>
{
    public List<T> Items { get; } = new();

    public int Page { get; }

    public int PageSize { get; }

    public int TotalItems { get; }

    public int TotalPages { get; }

    public PagedList(List<T> items, int page, int pageSize, int totalItems, int totalPages)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }
}
