using EStore.Contracts.Common;
using EStore.Contracts.Invoices;
using MediatR;

namespace EStore.Application.Invoices.Queries.GetInvoiceListPaged;

public record GetInvoiceListPagedQuery(
    int Page,
    int PageSize,
    string? Order,
    string? OrderBy)
    : IRequest<PagedList<InvoiceResponse>>;
