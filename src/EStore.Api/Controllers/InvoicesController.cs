using EStore.Application.Invoices.Queries.GetInvoiceListPaged;
using EStore.Contracts.Invoices;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class InvoicesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public InvoicesController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices([FromQuery] GetInvoiceListPagedRequest request)
    {
        var query = _mapper.Map<GetInvoiceListPagedRequest, GetInvoiceListPagedQuery>(request);
        var invoices = await _mediator.Send(query);

        return Ok(invoices);
    }
}
