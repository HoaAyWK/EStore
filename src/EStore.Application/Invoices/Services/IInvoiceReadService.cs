using EStore.Contracts.Common;
using EStore.Contracts.Invoices;

namespace EStore.Application.Invoices.Services;

public interface IInvoiceReadService
{
    Task<PagedList<InvoiceResponse>> GetListPagedAsync(
        int page,
        int pageSize,
        string? order,
        string? orderBy);
}
