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
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://api.aurumcheckout.com",
                ValidAudiences = ["urn:aurumpay:api"]
            };
        });
    }
}