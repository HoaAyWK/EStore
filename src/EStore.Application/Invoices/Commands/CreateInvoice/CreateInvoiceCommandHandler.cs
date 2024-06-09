using ErrorOr;
using EStore.Domain.InvoiceAggregate.Repositories;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;
using EStore.Domain.InvoiceAggregate;

namespace EStore.Application.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, ErrorOr<Created>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IOrderRepository _orderRepository;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IOrderRepository orderRepository)
    {
        _invoiceRepository = invoiceRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ErrorOr<Created>> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        var invoice = Invoice.Create(
            request.InvoiceBlobUrl,
            order.OrderNumber,
            order.Id,
            order.CustomerId);

        await _invoiceRepository.AddAsync(invoice);

        return Result.Created;
    }
}