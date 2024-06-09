using EStore.Contracts.Orders;

namespace EStore.Application.Invoices.Services;

public interface IInvoiceGenerator
{
    Task<byte[]> GenerateInvoiceAsync(OrderResponse order);
}
