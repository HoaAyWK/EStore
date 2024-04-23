using ErrorOr;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Application.Customers.Commands.UpdateAddress;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, ErrorOr<Address>>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateAddressCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<Address>> Handle(
        UpdateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        var addressId = AddressId.Create(request.AddressId);

        var updateAddressResult = customer.UpdateAddress(
            addressId,
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

        if (updateAddressResult.IsError)
        {
            return updateAddressResult.Errors;
        }

        return updateAddressResult.Value;
    }
}
