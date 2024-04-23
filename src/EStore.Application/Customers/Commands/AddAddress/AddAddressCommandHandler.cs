using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Customers.Commands.AddAddress;

public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, ErrorOr<Address>>
{
    private readonly ICustomerRepository _customerRepository;

    public AddAddressCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<Address>> Handle(
        AddAddressCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        var addAddressResult = customer.AddAddress(
            request.ReceiverName,
            request.PhoneNumber,
            request.IsDefault,
            request.Street,
            request.City,
            request.StateId,
            request.State,
            request.CountryId,
            request.Country,
            request.ZipCode);

        if (addAddressResult.IsError)
        {
            return addAddressResult.Errors;
        }

        return addAddressResult.Value;
    }
}
