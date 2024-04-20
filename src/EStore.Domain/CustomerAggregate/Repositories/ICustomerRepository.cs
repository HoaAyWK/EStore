using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Domain.CustomerAggregate.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(CustomerId id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
}
