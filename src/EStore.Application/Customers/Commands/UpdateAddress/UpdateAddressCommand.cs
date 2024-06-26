using ErrorOr;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Customers.Commands.UpdateAddress;

public record UpdateAddressCommand(
    CustomerId CustomerId,
    Guid AddressId,
    string ReceiverName,
    string PhoneNumber,
    bool IsDefault,
    string Street,
    string City,
    int StateId,
    string State,
    int CountryId,
    string Country,
    string ZipCode)
    : IRequest<ErrorOr<Address>>;
