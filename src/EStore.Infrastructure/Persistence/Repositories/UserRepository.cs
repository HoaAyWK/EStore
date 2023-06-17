using EStore.Domain.UserAggregate;
using EStore.Domain.UserAggregate.Repositories;
using EStore.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly EStoreDbContext _dbContext;

    public UserRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.AddAsync(user);
    }
    
    public async Task<User?> GetByIdAsync(UserId id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }
}
