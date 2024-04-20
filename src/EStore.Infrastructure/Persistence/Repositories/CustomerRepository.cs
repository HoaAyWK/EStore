using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly EStoreDbContext _dbContext;

    public CustomerRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Customer customer)
    {
        await _dbContext.AddAsync(customer);
    }
    
    public async Task<Customer?> GetByIdAsync(CustomerId id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbContext.Customers.Where(u => u.Email == email)
            .SingleOrDefaultAsync();
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbContext.Customers.Where(u => u.PhoneNumber == phoneNumber)
            .SingleOrDefaultAsync();
    }
}
