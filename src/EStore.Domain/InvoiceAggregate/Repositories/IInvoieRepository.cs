namespace EStore.Domain.InvoiceAggregate.Repositories;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice);
}