namespace EStore.Infrastructure.Persistence.Seeds;

public class UserSeedingSettings
{
    public const string SectionName = "Admin";

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;
}
