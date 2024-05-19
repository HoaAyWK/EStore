using ErrorOr;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Customers.Queries.GetCustomerByEmail;

public class GetCustomerByEmailQueryHandler
    : IRequestHandler<GetCustomerByEmailQuery, ErrorOr<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByEmailQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<CustomerResponse>> Handle(
        GetCustomerByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(request.Email);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        return new CustomerResponse(
            customer.Id.Value,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            customer.AvatarUrl,
            null,
            null);
    }
}
