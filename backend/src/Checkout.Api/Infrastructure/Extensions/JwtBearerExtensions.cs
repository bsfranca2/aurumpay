using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AurumPay.Checkout.Api.Infrastructure.Extensions;

public static class JwtBearerExtensions
{
    public static AuthenticationBuilder AddJwtBearerConfig(this AuthenticationBuilder builder)
    {
        return builder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Checkout.Api"
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    Claim? audienceClaim =
                        context.Principal?.Claims.FirstOrDefault(c => c.Type is JwtRegisteredClaimNames.Aud);

                    if (audienceClaim == null)
                    {
                        context.Fail("No audience claim found");
                        return Task.CompletedTask;
                    }

                    HttpContext httpContext = context.HttpContext;
                    string requestScheme = httpContext.Request.Scheme;
                    string? requestHost = httpContext.Request.Host.Value;

                    if (string.IsNullOrWhiteSpace(requestScheme) || string.IsNullOrWhiteSpace(requestHost))
                    {
                        context.Fail("Invalid request scheme or host");
                        return Task.CompletedTask;
                    }

                    string expectedAudience = $"{requestScheme}://{requestHost}";

                    if (!audienceClaim.Value.Equals(expectedAudience, StringComparison.OrdinalIgnoreCase))
                    {
                        context.Fail("Invalid audience");
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }
}