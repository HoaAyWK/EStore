using ErrorOr;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Customers.Queries;

public record GetCustomerByIdQuery(CustomerId CustomerId)
    : IRequest<ErrorOr<CustomerResponse>>;
