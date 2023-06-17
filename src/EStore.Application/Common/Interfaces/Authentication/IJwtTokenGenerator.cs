using ErrorOr;

namespace EStore.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    Task<ErrorOr<string>> GenerateTokenAsync(string userId);
}
