using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Customers.Command.UpdateCustomer;

public record UpdateCustomerCommand(
    CustomerId Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? AvatarUrl)
    : IRequest<ErrorOr<Updated>>;
