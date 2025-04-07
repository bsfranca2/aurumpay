using AurumPay.Application.Features.Checkout;
using AurumPay.Application.Infrastructure.Cache;
using AurumPay.Application.Infrastructure.Persistence;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AurumPay.Application;

public static class DependencyConfig
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyConfig).Assembly, includeInternalTypes: true);
        
        services.AddScoped<CheckoutSessionService>();

        return services;
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

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
            options.AddPolicy("ValidCheckoutSession", policy =>
                policy.RequireAssertion(ctx =>
                {
                    var httpContext = ctx.Resource as HttpContext;
                    if (httpContext == null) return false;

                    var sessionId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;
                    if (string.IsNullOrEmpty(sessionId)) return false;

                    var sessionService = httpContext.RequestServices
                        .GetRequiredService<CheckoutSessionService>();
                
                    return sessionService.ValidateSessionAsync(sessionId);
                }));
        });
        
        services.AddHttpContextAccessor();

        return services;
    }
}