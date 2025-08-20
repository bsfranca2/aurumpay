using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using AurumPay.Checkout.Api.Infrastructure.Options;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using NetDevPack.Security.Jwt.Core.Interfaces;

using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class CheckoutSessionManager(
    IHttpContextAccessor httpContextAccessor,
    ICheckoutSessionRepository checkoutSessionRepository,
    IJwtService jwtService,
    ICustomerRepository customerRepository,
    IOptions<JwtOptions> jwtOptions
) : ICheckoutSessionManager
{
    private const string CheckoutSessionHeaderKey = "Authorization";
    private CheckoutSession? _currentSessionCached;
    private Customer? _currentCustomerCached;

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

        _currentSessionCached ??= await checkoutSessionRepository.GetByIdAsync(sessionId.Value);

        return _currentSessionCached;
    }

    public async Task<Customer?> GetCurrentCustomerAsync()
    {
        CheckoutSession? session = await GetCurrentSessionAsync();

        if (session?.CustomerId == null)
        {
            return null;
        }

        _currentCustomerCached ??= await customerRepository.GetByIdWithAddressesAsync(session.CustomerId.Value);

        return _currentCustomerCached;
    }

    public async Task EstablishSessionAsync(CheckoutSession checkoutSession)
    {
        HttpContext context = httpContextAccessor.HttpContext!;
        
        string issuer = jwtOptions.Value.Issuer;
        string audience = jwtOptions.Value.Audience;
        int lifetime = jwtOptions.Value.TokenLifetimeMinutes;
        
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddMinutes(lifetime),
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, checkoutSession.Id.Value.ToString())
            ]),
            SigningCredentials = await jwtService.GetCurrentSigningCredentials()
        };
        
        JwtSecurityTokenHandler tokenHandler = new();
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