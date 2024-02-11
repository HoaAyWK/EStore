using Microsoft.EntityFrameworkCore;
using EStore.Infrastructure.Persistence;

namespace EStore.Infrastructure.Identity;

public sealed class AccountTokenService : IAccountTokenService
{
    private readonly EStoreDbContext _dbContext;

    public AccountTokenService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAccountTokenAsync(AccountToken accountToken)
    {
        await _dbContext.AccountTokens.AddAsync(accountToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTokenAsync(AccountToken accountToken)
    {
        _dbContext.AccountTokens.Remove(accountToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AccountToken?> GetAccountTokenAsync(
        string token,
        string email)
    {
        return await _dbContext.AccountTokens
            .Where(t => t.Token == token && t.Email == email)
            .SingleOrDefaultAsync();
    }
}
