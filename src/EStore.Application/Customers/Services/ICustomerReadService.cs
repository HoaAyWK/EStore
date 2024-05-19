using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Application.Customers.Services;

public interface ICustomerReadService
{
    Task<CustomerStats> GetCustomerStatsAsync(int lastDaysCount);
    Task<List<CustomerResponse>> GetCustomersAsync(CustomerId currentCustomerId);
}
