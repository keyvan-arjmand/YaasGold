using GoldShop.Application.Interfaces;
using GoldShop.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GoldShop.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();



        return services;
    }
}