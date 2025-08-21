using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

using Microsoft.AspNetCore.Authorization;

namespace AurumPay.Infrastructure.Authorization;

public class CheckoutSessionOrderHandler(ICheckoutSessionManager sessionManager)
    : AuthorizationHandler<CheckoutSessionOrderRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CheckoutSessionOrderRequirement requirement)
    {
        CheckoutSession? session = await sessionManager.GetCurrentSessionAsync();

        if (session?.OrderId != null)
        {
            context.Succeed(requirement);
        }
    }
}