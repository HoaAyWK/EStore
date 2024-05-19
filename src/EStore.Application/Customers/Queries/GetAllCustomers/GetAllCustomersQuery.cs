using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Customers.Queries.GetAllCustomers;

public record GetAllCustomersQuery(CustomerId CurrentCustomerId)
    : IRequest<List<CustomerResponse>>;
