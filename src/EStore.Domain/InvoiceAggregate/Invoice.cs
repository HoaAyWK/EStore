using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.InvoiceAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.InvoiceAggregate;

public sealed class Invoice : AggregateRoot<InvoiceId>, ISoftDeletableEntity, IAuditableEntity
{
    public string InvoiceBlobUrl { get; private set; } = default!;

    public long InvoiceNumber { get; private set; }

    public OrderId OrderId { get; private set; } = default!;

    public CustomerId CustomerId { get; private set; } = default!;

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private Invoice()
    {
    }

    private Invoice(
        InvoiceId id,
        string invoiceBlobUrl,
        long invoiceNumber,
        OrderId orderId,
        CustomerId customerId)
        : base(id)
    {
        InvoiceBlobUrl = invoiceBlobUrl;
        InvoiceNumber = invoiceNumber;
        OrderId = orderId;
        CustomerId = customerId;
    }

    public static Invoice Create(
        string invoiceBlobUrl,
        long invoiceNumber,
        OrderId orderId,
        CustomerId customerId)
        => new(InvoiceId.CreateUnique(),
            invoiceBlobUrl,
            invoiceNumber,
            orderId,
            customerId);
}