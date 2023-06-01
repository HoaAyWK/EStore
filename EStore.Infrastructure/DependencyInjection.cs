using EStore.Application.Common.Interfaces.Authentication;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Persistence;
using EStore.Infrastructure.Persistence.Repositories;
using EStore.Infrastructure.Services;
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
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductAttributeReadRepository, ProductAttributeReadRepository>();

        services.AddDbContext<EStoreDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")!));

        return services;
    }   
}