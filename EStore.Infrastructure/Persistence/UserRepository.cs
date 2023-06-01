using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Entities;

namespace EStore.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly static List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }
}
