using ErrorOr;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Customers.Commands.AddAddress;

public record AddAddressCommand(
    CustomerId CustomerId,
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
