using EStore.Domain.InvoiceAggregate;
using EStore.Domain.InvoiceAggregate.Repositories;

namespace EStore.Infrastructure.Persistence.Repositories;

public sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly EStoreDbContext _dbContext;

    public InvoiceRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Invoice invoice)
        => await _dbContext.Invoices.AddAsync(invoice);
}
