using EStore.Application.Carts.Services;
using EStore.Application.Common.Interfaces.Authentication;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Discounts.Services;
using EStore.Application.Orders.Services;
using EStore.Application.Products.Services;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Authentication.OptionsSetup;
using EStore.Infrastructure.BackgroundJobs;
using EStore.Infrastructure.Identity;
using EStore.Infrastructure.Messaging;
using EStore.Infrastructure.Persistence;
using EStore.Infrastructure.Persistence.Interceptors;
using EStore.Infrastructure.Persistence.Repositories;
using EStore.Infrastructure.Persistence.Seeds;
using EStore.Infrastructure.Services;
using EStore.Infrastructure.Services.AlgoliaSearch;
using EStore.Infrastructure.Services.OptionsSetup;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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
        services.ConfigureOptions<StripeSettingsOptionsSetup>();

        services.AddMediatR(typeof(DependencyInjection).Assembly);

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
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();

        services.AddScoped<IBrandReadService, BrandReadService>();
        services.AddScoped<ICategoryReadService, CategoryReadService>();
        services.AddScoped<IProductReadService, ProductReadService>();
        services.AddScoped<ICartReadService, CartReadService>();
        services.AddScoped<IOrderReadService, OrderReadService>();
        services.AddScoped<IDiscountReadService, DiscountReadService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPriceCalculationService, PriceCalculationService>();
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddDbContext<EStoreDbContext>((sp, options) =>
        {
            var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!)
                .AddInterceptors(interceptor!);
        });

        services.AddDbContext<EStoreIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AppIdentityConnection")!));

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(10)
                                .RepeatForever()));
        });

        services.AddQuartzHostedService();

        services.AddAlgoliaSearch(configuration);

        return services;
    }   
}
