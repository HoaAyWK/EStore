namespace EStore.Contracts.Invoices;

public record GetInvoiceListPagedRequest(
    int Page,
    int PageSize,
    string? Order,
    string? OrderBy);
