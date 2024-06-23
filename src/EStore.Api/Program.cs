using EStore.Api;
using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Options;
using EStore.Application;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Infrastructure;
using EStore.Infrastructure.Identity;
using EStore.Infrastructure.Persistence;
using EStore.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation(builder.Configuration)
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var corsPolicyName = builder.Configuration
    .GetSection(CorsOptions.SectionName)
    .Get<CorsOptions>()?.PolicyName ?? "DefaultPolicy";

var app = builder.Build();
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    app.UseExceptionHandler($"/{ApiRoutes.Error}");
    // app.UseHttpsRedirection();
    app.UseCors(corsPolicyName);
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles();
    app.MapControllers();
    app.MapHub<NotificationsHub>(NotificationsHub.HubUrl);
    app.MapHealthChecks("/api/health");

    await SeedDataAsync(app);

    app.Run();
}

async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    try
    {
        var eStoreContext = scope.ServiceProvider.GetRequiredService<EStoreDbContext>();
        var identityContext = scope.ServiceProvider.GetRequiredService<EStoreIdentityDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<UserSeedingSettings>>();

        await EStoreIdentityDbContextSeed.SeedAsync(
            eStoreContext,
            identityContext,
            unitOfWork,
            userManager,
            roleManager,
            options);

        await EStoreDbContextSeed.SeedAsync(eStoreContext, unitOfWork);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while seeding data.");
    }
}
