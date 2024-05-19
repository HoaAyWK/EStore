using EStore.Application.Customers.Services;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Identity;
using EStore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Searching;

public class CustomerReadService : ICustomerReadService
{
    private readonly EStoreDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerReadService(
        EStoreDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<List<CustomerResponse>> GetCustomersAsync(CustomerId currentCustomerId)
    {
        var customerResponses = new List<CustomerResponse>();
        var customers = await _dbContext.Customers.AsNoTracking()
            .Where(customer => customer.Id != currentCustomerId)
            .ToListAsync();
        
        foreach (var customer in customers)
        {
            var user = await _userManager.FindByIdAsync(customer.Id.Value.ToString());
            
            if (user is null)
            {
                continue;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.Count > 0 ? roles[0] : Roles.Customer;

            customerResponses.Add(new CustomerResponse(
                customer.Id.Value,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber,
                customer.AvatarUrl,
                userRole,
                user.EmailConfirmed));
        }

        return customerResponses;
    }

    public async Task<CustomerStats> GetCustomerStatsAsync(int lastDaysCount)
    {
        var totalCustomers = await _dbContext.Customers.CountAsync();
        var customerGroups = await _dbContext.Customers
            .Where(order => order.CreatedDateTime.Date >= DateTime.UtcNow.Date.AddDays(-lastDaysCount))
            .GroupBy(order => order.CreatedDateTime.Date)
            .Select(group => new
            {
                Date = group.Key,
                Count = group.Count()
            })
            .ToListAsync();

        var startDate = DateTime.UtcNow.Date.AddDays(-lastDaysCount);
        var endDate = DateTime.UtcNow.Date;
        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(offset => startDate.AddDays(offset));

        var customerGroupWithDefault = from date in dateRange
            join orderGroup in customerGroups
            on date equals orderGroup.Date into gj
            from subOrderGroup in gj.DefaultIfEmpty()
            select new
            {
                Date = date,
                Count = subOrderGroup != null ? subOrderGroup.Count : 0
            };

        var customerRegistersPerDay = customerGroupWithDefault
            .OrderBy(group => group.Date)
            .Select(group => group.Count)
            .ToList();

        var lastPreviousCustomerRegistersPerDay = await _dbContext.Customers
            .Where(order =>
                order.CreatedDateTime.Date >= DateTime.UtcNow.Date.AddDays(-lastDaysCount * 2) &&
                order.CreatedDateTime.Date < DateTime.UtcNow.Date.AddDays(-lastDaysCount))
            .GroupBy(order => order.CreatedDateTime.Date)
            .Select(group => new
            {
                Date = group.Key,
                Count = group.Count()
            })
            .OrderBy(group => group.Date)
            .Select(group => group.Count)
            .ToListAsync();

        var totalCustomerRegistersFromLastDays = customerRegistersPerDay.Sum();
        var totalCustomerRegistersFromPreviousDays = lastPreviousCustomerRegistersPerDay.Sum();
        var compareToPreviousDays = 0.0;

        try
        {
            compareToPreviousDays = (totalCustomerRegistersFromLastDays - totalCustomerRegistersFromPreviousDays)
                / (double)totalCustomerRegistersFromPreviousDays * 100;
        }
        catch (DivideByZeroException)
        {
            compareToPreviousDays = totalCustomerRegistersFromLastDays / 1.0 * 100;
        }
        
        
        var isIncreased = compareToPreviousDays > 0;

        return new CustomerStats
        {
            TotalCustomers = totalCustomers,
            CustomerRegistersPerDay = customerRegistersPerDay,
            FromDay = lastDaysCount,
            CompareToPreviousDays = compareToPreviousDays,
            IsIncreased = isIncreased
        };
    }
}
