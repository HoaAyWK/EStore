using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Invoices.Commands.CreateInvoice;

public record CreateInvoiceCommand(
    OrderId OrderId,
    string InvoiceBlobUrl)
    : IRequest<ErrorOr<Created>>;