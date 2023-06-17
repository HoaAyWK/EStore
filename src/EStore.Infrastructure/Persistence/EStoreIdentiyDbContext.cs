using EStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence;

public class EStoreIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public EStoreIdentityDbContext(DbContextOptions<EStoreIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
