using EStore.Application.Common.Interfaces.Persistence;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Persistence.Seeds;

public class EStoreIdentityDbContextSeed
{
    public static async Task SeedAsync(
        EStoreDbContext eStoreContext,
        EStoreIdentityDbContext identityContext,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<UserSeedingSettings> userSeedingSettingsOptions)
    {
        if (eStoreContext.Database.IsSqlServer())
        {
            await eStoreContext.Database.MigrateAsync();
        }

        if (identityContext.Database.IsSqlServer())
        {
            await identityContext.Database.MigrateAsync();
        }

        if (await roleManager.FindByNameAsync(Roles.Admin) is null)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        if (await roleManager.FindByNameAsync(Roles.Customer) is null)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
        }

        var adminSettings = userSeedingSettingsOptions.Value;

        var admin = await eStoreContext.Customers
            .Where(x => x.Email == adminSettings.Email)
            .FirstOrDefaultAsync();

        if (admin is null)
        {
            admin = Domain.CustomerAggregate.Customer.Create(
                adminSettings.Email,
                adminSettings.FirstName,
                adminSettings.LastName).Value;

            admin.UpdateDetails(
                adminSettings.FirstName,
                adminSettings.LastName,
                adminSettings.Phone,
                adminSettings.AvatarUrl,
                adminSettings.Street,
                adminSettings.City,
                adminSettings.State,
                adminSettings.Country);
            
            eStoreContext.Customers.Add(admin);

            await unitOfWork.SaveChangesAsync();

            admin = await eStoreContext.Customers
                .Where(x => x.Email == adminSettings.Email)
                .FirstOrDefaultAsync();
    
            if (admin is not null)
            {
                ApplicationUser? appAdmin = new()
                {
                    Id = admin.Id.Value.ToString(),
                    UserName = adminSettings.Email,
                    Email = adminSettings.Email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(appAdmin, adminSettings.Password);

                appAdmin = await userManager.FindByEmailAsync(adminSettings.Email);

                if (appAdmin is not null)
                {
                    await userManager.AddToRoleAsync(appAdmin, Roles.Admin);
                }
            }
        }
    }
}
