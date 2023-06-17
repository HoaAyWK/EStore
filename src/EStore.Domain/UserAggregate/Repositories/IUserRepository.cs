using EStore.Domain.UserAggregate.ValueObjects;

namespace EStore.Domain.UserAggregate.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(UserId id);
    Task<User?> GetByEmailAsync(string email);
}
