using Microsoft.Extensions.DependencyInjection;
using OnZeroId.Application.Mappings;


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

        return services;
    }
}