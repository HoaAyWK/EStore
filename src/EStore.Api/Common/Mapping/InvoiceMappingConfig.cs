using EStore.Application.Invoices.Queries.GetInvoiceListPaged;
using EStore.Contracts.Invoices;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class InvoiceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetInvoiceListPagedRequest, GetInvoiceListPagedQuery>();
    }
}
