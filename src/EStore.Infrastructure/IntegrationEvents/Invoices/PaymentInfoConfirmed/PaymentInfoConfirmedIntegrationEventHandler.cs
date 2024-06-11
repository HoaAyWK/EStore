using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Invoices.Commands.CreateInvoice;
using EStore.Application.Invoices.Services;
using EStore.Application.Orders.Events;
using EStore.Application.Orders.Services;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace EStore.Infrastructure.IntegrationEvents.Invoices.PaymentInfoConfirmed;

public class PaymentInfoConfirmedIntegrationEventHandler
    : INotificationHandler<PaymentInfoConfirmedIntegrationEvent>
{
    private readonly IOrderReadService _orderReadService;
    private readonly IInvoiceGenerator _invoiceGenerator;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ISender _mediator;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PaymentInfoConfirmedIntegrationEventHandler(
        IOrderReadService orderReadService,
        IInvoiceGenerator invoiceGenerator,
        ICloudinaryService cloudinaryService,
        ISender mediator,
        IEmailService emailService,
        IWebHostEnvironment webHostEnvironment)
    {
        _orderReadService = orderReadService;
        _invoiceGenerator = invoiceGenerator;
        _cloudinaryService = cloudinaryService;
        _mediator = mediator;
        _emailService = emailService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Handle(
        PaymentInfoConfirmedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var order = await _orderReadService.GetByIdAsync(notification.OrderId);

        if (order is null)
        {
            // TODO: log error

            return;
        }

        var bytes = await _invoiceGenerator.GenerateInvoiceAsync(order);
        var fileName = $"Invoice_{order.Id}.pdf";
        var invoiceBlobUrl = await _cloudinaryService.UploadFileAsync(bytes, fileName);
        var command = new CreateInvoiceCommand(
            OrderId.Create(order.Id),
            invoiceBlobUrl);

        var createInvoiceResult = await _mediator.Send(command, cancellationToken);

        if (createInvoiceResult.IsError)
        {
            // TODO: log errors

            return;
        }

        if (order.Customer?.Email is not null)
        {
            var htmlElement = $"<a href={invoiceBlobUrl}>here</a>";
            var htmlBody = await File.ReadAllTextAsync(
                GetTemplatePath(),
                cancellationToken);
            
            htmlBody = htmlBody.Replace("{0}", htmlElement);

            await _emailService.SendEmailWithTemplateAsync(
                subject: "[EStore] Your order has been confirmed.",
                mailTo: order.Customer.Email,
                htmlBody: htmlBody);
        }
    }

    private string GetTemplatePath()
    {
        var separator = Path.DirectorySeparatorChar.ToString();

        return _webHostEnvironment.WebRootPath
            + separator
            + "Templates"
            + separator
            + "invoice.html";
    }
}
