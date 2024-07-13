using ErrorOr;
using EStore.Domain.CustomerAggregate;
using MediatR;

namespace EStore.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? AvatarUrl)
    : IRequest<ErrorOr<Customer>>;
