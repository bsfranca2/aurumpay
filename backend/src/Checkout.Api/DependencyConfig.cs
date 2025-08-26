using AurumPay.Application;
using AurumPay.Application.Data;
using AurumPay.Checkout.Api.Infrastructure.Extensions;
using AurumPay.Checkout.Api.Infrastructure.Options;
using AurumPay.Checkout.Api.Infrastructure.Services;
using AurumPay.Domain.Interfaces;
using AurumPay.Infrastructure;
using AurumPay.Infrastructure.Authorization;
using AurumPay.Infrastructure.EntityFramework;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.StackExchangeRedis;

using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace AurumPay.Checkout.Api;

public static class DependencyConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.AddMemoryCache();

        services.SetupEntityFramework();
        services.AddEfRepositories();
        services.AddScoped<IDatabaseContext>(sp => sp.GetRequiredService<DatabaseContext>());

        services.AddFusionCache()
            .WithDefaultEntryOptions(options => options.Duration = TimeSpan.FromMinutes(5))
            .WithSerializer(new FusionCacheSystemTextJsonSerializer())
            .WithDistributedCache(new RedisCache(new RedisCacheOptions
            {
                Configuration = configuration.GetConnectionString("Cache")
            }));

        services.AddHttpContextAccessor();

        services.AddForwardedHeadersConfig(environment, configuration);

        services.AddScoped<IAuthorizationHandler, CheckoutSessionCustomerHandler>();
        services.AddScoped<IAuthorizationHandler, CheckoutSessionOrderHandler>();

        services.ConfigureOptions<AuthorizationOptionsSetup>();
        services.ConfigureOptions<JwtOptionsSetup>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearerConfig();
        services.AddAuthorization();
        services.AddJwksManager().UseJwtValidation();

        services.AddCatalogServices(configuration);
        services.AddPaymentServices(configuration);

        services.ConfigureOptions<SecurityOptionsSetup>();
        services.AddScoped<IStoreContext, StoreContext>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICheckoutSessionManager, CheckoutSessionManager>();
        services.AddScoped<IDeviceIdentityProvider, RequestDeviceIdentityService>();
        services.AddScoped<ICheckoutContext, CheckoutContext>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyReference).Assembly);
        });
        services.AddMediatRLoggingBehavior();
        services.AddMediatRFluentValidationBehavior();
        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

        return services;
    }
}