using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

using Microsoft.IdentityModel.Tokens;

using NetDevPack.Security.Jwt.Core.Interfaces;

using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class CheckoutSessionManager(
    IHttpContextAccessor httpContextAccessor,
    ICheckoutSessionRepository checkoutSessionRepository,
    IJwtService jwtService
) : ICheckoutSessionManager
{
    private const string CheckoutSessionHeaderKey = "Authorization";
    
    public CheckoutSessionId? GetCurrentSessionId()
    {
        Claim? claim = httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        if (claim?.Value is null)
        {
            return null;
        }

        return Ulid.TryParse(claim.Value, out Ulid parsedId)
            ? new CheckoutSessionId(parsedId)
            : null;
    }

    public async Task<CheckoutSession?> GetCurrentSessionAsync()
    {
        CheckoutSessionId? sessionId = GetCurrentSessionId();

        if (sessionId is null)
        {
            return null;
        }

        return await checkoutSessionRepository.GetByIdAsync(sessionId.Value);
    }

    public async Task EstablishSessionAsync(CheckoutSession checkoutSession)
    {
        HttpContext context = httpContextAccessor.HttpContext!;
        string requestScheme = context.Request.Scheme;
        string? requestHost = context.Request.Host.Value;

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = "Checkout.Api",
            Audience =  $"{requestScheme}://{requestHost}",
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, checkoutSession.Id.Value.ToString())
            ]),
            SigningCredentials = await jwtService.GetCurrentSigningCredentials()
        };
        SecurityToken? jwt = tokenHandler.CreateToken(tokenDescriptor);
        string? jws = tokenHandler.WriteToken(jwt);

        context.Response.Headers.Append(CheckoutSessionHeaderKey, $"Bearer {jws}");
    }

    public async Task EndSessionAsync()
    {
        HttpContext? context = httpContextAccessor.HttpContext;
        context?.Response.Headers.Append(CheckoutSessionHeaderKey, "");

        await Task.CompletedTask;
    }
}