using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EStore.Api.Common.OptionsSetup;

public class SwaggerGenOptionsSetup : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "EStore.Api", Version = "v1" });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                            Enter 'Bearer' [space] and then your token in the text input below.
                            Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    }
}
