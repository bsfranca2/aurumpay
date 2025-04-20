using System.Security.Claims;

using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class CheckoutSessionManager(
    IHttpContextAccessor httpContextAccessor,
    ICheckoutSessionRepository checkoutSessionRepository
) : ICheckoutSessionManager
{
    public CheckoutSessionId? GetCurrentSessionId()
    {
        Claim? claim = httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == InfraConstants.CheckoutSessionIdClaimType);

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
        await context.SignInAsync(
            new ClaimsPrincipal(new ClaimsIdentity(
                [new Claim(InfraConstants.CheckoutSessionIdClaimType, checkoutSession.Id.Value.ToString())],
                "CheckoutAuth")),
            new AuthenticationProperties { IsPersistent = true, AllowRefresh = true });
    }

    public async Task EndSessionAsync()
    {
        // TODO: Testar, criar sessão, apagar sessão no banco de dados e fazer a request
        HttpContext? context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}