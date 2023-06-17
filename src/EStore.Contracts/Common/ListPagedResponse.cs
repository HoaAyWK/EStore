namespace EStore.Contracts.Common;

public abstract class ListPagedResponse<T>
{
    public List<T> Data { get; set; } = new();

    public int PageSize { get; set; }

    public int Page { get; set; }

    public int TotalItems { get; set; }
}
