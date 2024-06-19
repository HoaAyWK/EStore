using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Customers.Commands.DeleteAddress;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, ErrorOr<Deleted>>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteAddressCommandHandler(
        ICustomerRepository customerRepository)
        => _customerRepository = customerRepository;

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteAddressCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        return customer.DeleteAddress(request.AddressId);
    }
}
