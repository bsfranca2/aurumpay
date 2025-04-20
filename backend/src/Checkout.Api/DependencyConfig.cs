using AurumPay.Application;
using AurumPay.Application.Data;
using AurumPay.Checkout.Api.Infrastructure.Cache;
using AurumPay.Checkout.Api.Infrastructure.Options;
using AurumPay.Checkout.Api.Infrastructure.Services;
using AurumPay.Domain.Interfaces;
using AurumPay.Infrastructure.EntityFramework;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace AurumPay.Checkout.Api;

public static class DependencyConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetupEntityFramework();
        services.AddEfRepositories();
        
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, InMemoryCacheService>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "CheckoutSession";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;

                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddAuthorization(options =>
        {
            // options.AddPolicy("ValidCheckoutSession", policy =>
            //     policy.RequireAssertion(ctx =>
            //     {
            //         var httpContext = ctx.Resource as HttpContext;
            //         if (httpContext == null) return false;
            //
            //         var sessionId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;
            //         if (string.IsNullOrEmpty(sessionId)) return false;
            //
            //         var sessionService = httpContext.RequestServices
            //             .GetRequiredService<CheckoutSessionService>();
            //     
            //         return sessionService.ValidateSessionAsync(sessionId);
            //     }));
        });
        
        services.AddHttpContextAccessor();

        services.AddScoped<IDatabaseContext>(sp => sp.GetRequiredService<DatabaseContext>());
        
        services.ConfigureOptions<SecurityOptionsSetup>();
        
        services.AddScoped<IStoreContext, StoreContext>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICheckoutSessionManager, CheckoutSessionManager>();
        services.AddScoped<IDeviceIdentityProvider, RequestDeviceIdentityService>();
        services.AddScoped<ICheckoutContext, CheckoutContext>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies([typeof(ApplicationAssemblyReference).Assembly]);
        });

        return services;
    }
}