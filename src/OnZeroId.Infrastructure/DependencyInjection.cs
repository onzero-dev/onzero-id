// src/OnZeroId.Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnZeroId.Domain.Interfaces.Repositories;
using OnZeroId.Infrastructure.Persistence.DbContexts;
using OnZeroId.Infrastructure.Persistence.Mappings;
using OnZeroId.Infrastructure.Persistence.Repositories;
using OnZeroId.Infrastructure.Services;
using Minio;
using OnZeroId.Application.Interfaces;


namespace OnZeroId.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 設定 DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<OnZeroIdDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 設定 AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(InfraUserToDomainUserProfile).Assembly);
            cfg.AddMaps(typeof(InfraTotpKeyToDomainTotpKeyProfile).Assembly);
        });

        // 註冊 Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITotpKeyRepository, TotpKeyRepository>();
        // services.AddScoped<IPasskeyRepository, PasskeyRepository>();

        // Minio 設定與 DI
        var rawConfig = configuration.GetSection("Minio");
        var minioOptions = rawConfig.Get<MinioOptions>();
        if (minioOptions == null)
        {
            throw new InvalidOperationException("Minio configuration is missing.");
        }
        services.Configure<MinioOptions>(rawConfig);
        services.AddMinio(configureClient =>
        {
            var endpoint = minioOptions.Endpoint;
            var accessKey = minioOptions.AccessKey;
            var secretKey = minioOptions.SecretKey;
            var region = minioOptions.Region;
            configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithRegion(region);
        });

        // QrCode Service DI
        services.AddScoped<IQrCodeService, QrCodeService>();
        services.AddScoped<IMinioService, MinioService>();

        return services;
    }
}