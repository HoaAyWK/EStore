using EStore.Domain.Entities;

namespace EStore.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
