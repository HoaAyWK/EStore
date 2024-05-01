using ErrorOr;
using EStore.Contracts.Customers;
using MediatR;

namespace EStore.Application.Customers.Queries.GetCustomerByEmail;

public record GetCustomerByEmailQuery(string Email)
    : IRequest<ErrorOr<CustomerResponse>>;
