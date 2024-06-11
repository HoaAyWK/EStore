using EStore.Application.Invoices.Services;
using EStore.Contracts.Common;
using EStore.Contracts.Invoices;
using MediatR;

namespace EStore.Application.Invoices.Queries.GetInvoiceListPaged;

public class GetInvoiceListPagedQueryHandler
    : IRequestHandler<GetInvoiceListPagedQuery, PagedList<InvoiceResponse>>
{
    private readonly IInvoiceReadService _invoiceReadService;

    public GetInvoiceListPagedQueryHandler(
        IInvoiceReadService invoiceReadService)
    {
        _invoiceReadService = invoiceReadService;
    }

    public async Task<PagedList<InvoiceResponse>> Handle(
        GetInvoiceListPagedQuery request,
        CancellationToken cancellationToken)
        => await _invoiceReadService.GetListPagedAsync(
            request.Page,
            request.PageSize,
            request.Order,
            request.OrderBy);
}
