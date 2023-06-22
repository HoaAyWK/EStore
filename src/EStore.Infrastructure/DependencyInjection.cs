using EStore.Application.Common.Interfaces.Authentication;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Services;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductVariantAggregate.Repositories;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Authentication.OptionsSetup;
using EStore.Infrastructure.Identity;
using EStore.Infrastructure.Persistence;
using EStore.Infrastructure.Persistence.Repositories;
using EStore.Infrastructure.Persistence.Seeds;
using EStore.Infrastructure.Services;
using EStore.Infrastructure.Services.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.ConfigureOptions<JwtSettingsOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<UserSeedingOptionsSetup>();
        services.ConfigureOptions<MailSettingsOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();

        services.AddAuthorization();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<EStoreIdentityDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IBrandReadService, BrandReadService>();
        services.AddScoped<ICategoryReadService, CategoryReadService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IProductReadService, ProductReadService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddDbContext<EStoreDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")!));

        services.AddDbContext<EStoreIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AppIdentityConnection")!));

        return services;
    }   
}
