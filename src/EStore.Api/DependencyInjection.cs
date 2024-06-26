using EStore.Api.Common.Contexts;
using EStore.Api.Common.Mapping;
using EStore.Api.Common.Options;
using EStore.Api.Common.OptionsSetup;
using EStore.Api.SignalRServices;
using EStore.Infrastructure.Services.SignalRServices;

namespace EStore.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection(CorsOptions.SectionName)
            .Get<CorsOptions>();

        services.ConfigureOptions<SwaggerGenOptionsSetup>();

        services.AddSignalR();

        services.AddMappings();
        services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();
        
        services.AddScoped<IWorkContext, WorkContext>();
        services.AddScoped<IWorkContextSource, WorkContextSource>();
        services.AddScoped<INotificationSignalR, NotificationSignalR>();

        if (corsOptions != null)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: corsOptions.PolicyName,
                    builder =>
                    {
                        builder.WithOrigins(corsOptions.AllowedOrigins.Split(","))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
        }

        // services.AddSingleton<ProblemDetailsFactory, EStoreProblemDetailsFactory>();

        services.AddHealthChecks();

        return services;
    }
}
