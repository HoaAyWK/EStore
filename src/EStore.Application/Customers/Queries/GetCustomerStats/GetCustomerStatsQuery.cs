using EStore.Contracts.Customers;
using MediatR;

namespace EStore.Application.Customers.Queries.GetCustomerStats;

public record GetCustomerStatsQuery(int FromDays)
    : IRequest<CustomerStats>;
