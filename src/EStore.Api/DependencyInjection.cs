using EStore.Api.Common.Mapping;

namespace EStore.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();
        services.AddControllers()
        .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore);
                
        // services.AddSingleton<ProblemDetailsFactory, EStoreProblemDetailsFactory>();

        return services;
    }
}
