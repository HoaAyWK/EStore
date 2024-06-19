using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Customers.Commands.DeleteAddress;

public record DeleteAddressCommand(
    CustomerId CustomerId,
    AddressId AddressId)
    : IRequest<ErrorOr<Deleted>>;
