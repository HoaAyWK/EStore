namespace EStore.Infrastructure.Persistence.Seeds;

public class UserSeedingSettings
{
    public const string SectionName = "Admin";

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public string AvatarUrl { get; init; } = null!;

    public string Phone { get; init; } = null!;

    public string Street { get; init; } = null!;

    public string City { get; init; } = null!;

    public string State { get; init; } = null!;

    public string Country { get; init; } = null!;
}
