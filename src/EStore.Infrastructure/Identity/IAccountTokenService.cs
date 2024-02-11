namespace EStore.Infrastructure.Identity;

public interface IAccountTokenService
{
    Task AddAccountTokenAsync(AccountToken accountToken);
    Task<AccountToken?> GetAccountTokenAsync(string token, string email);
    Task DeleteTokenAsync(AccountToken accountToken);
}
