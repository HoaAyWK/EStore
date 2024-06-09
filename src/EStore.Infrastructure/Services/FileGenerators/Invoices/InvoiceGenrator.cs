using EStore.Application.Invoices.Services;
using EStore.Contracts.Orders;
using EStore.Infrastructure.Services.FileGenerators.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EStore.Infrastructure.Services.FileGenerators;

public class InvoiceGenerator : IInvoiceGenerator
{
    public async Task<byte[]> GenerateInvoiceAsync(OrderResponse order)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        await Task.CompletedTask;
        
        var document = new InvoiceDocument(order);
        
        return Document.Create(document.Compose).GeneratePdf();
    }
}
