using EStore.Application.Customers.Services;
using EStore.Contracts.Customers;
using MediatR;

namespace EStore.Application.Customers.Queries.GetCustomerStats;

public class GetCustomerStatsQueryHandler : IRequestHandler<GetCustomerStatsQuery, CustomerStats>
{
    private readonly ICustomerReadService _customerReadService;

    public GetCustomerStatsQueryHandler(ICustomerReadService customerReadService)
    {
        _customerReadService = customerReadService;
    }

    public async Task<CustomerStats> Handle(
        GetCustomerStatsQuery request,
        CancellationToken cancellationToken)
    {
        return await _customerReadService.GetCustomerStatsAsync(request.FromDays);
    }
}
