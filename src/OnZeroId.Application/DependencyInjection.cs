using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using OnZeroId.Application.Mappings;
using OnZeroId.Application.Options;

namespace OnZeroId.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // 註冊 AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UserProfile).Assembly);
        });

        // 註冊 MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}