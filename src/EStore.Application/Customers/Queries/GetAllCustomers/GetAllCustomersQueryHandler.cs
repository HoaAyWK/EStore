using EStore.Application.Customers.Services;
using EStore.Contracts.Customers;
using MediatR;

namespace EStore.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQueryHandler
    : IRequestHandler<GetAllCustomersQuery, List<CustomerResponse>>
{
    private readonly ICustomerReadService _customerReadService;

    public GetAllCustomersQueryHandler(ICustomerReadService customerReadService)
    {
        _customerReadService = customerReadService;
    }

    public async Task<List<CustomerResponse>> Handle(
        GetAllCustomersQuery request,
        CancellationToken cancellationToken)
        => await _customerReadService.GetCustomersAsync(request.CurrentCustomerId);
}
