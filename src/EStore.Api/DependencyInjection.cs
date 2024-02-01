using EStore.Api.Common.Contexts;
using EStore.Api.Common.Mapping;
using EStore.Api.Common.Options;
using EStore.Api.Common.OptionsSetup;
using Microsoft.IdentityModel.Protocols;

namespace EStore.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection(CorsOptions.SectionName)
            .Get<CorsOptions>();

        services.ConfigureOptions<SwaggerGenOptionsSetup>();

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

        if (corsOptions != null)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: corsOptions.PolicyName,
                    builder =>
                    {
                        builder.WithOrigins(corsOptions.AllowedOrigins.Split(","))
                            .WithMethods(corsOptions.AllowedMethods.Split(","))
                            .WithHeaders(corsOptions.AllowedHeaders.Split(","));
                    });
            });
        }

        // services.AddSingleton<ProblemDetailsFactory, EStoreProblemDetailsFactory>();

        return services;
    }
}
