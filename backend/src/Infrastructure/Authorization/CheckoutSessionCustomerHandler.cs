using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

using Microsoft.AspNetCore.Authorization;

namespace AurumPay.Infrastructure.Authorization;

public class CheckoutSessionCustomerHandler(ICheckoutSessionManager sessionManager)
    : AuthorizationHandler<CheckoutSessionCustomerRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CheckoutSessionCustomerRequirement requirement)
    {
        CheckoutSession? session = await sessionManager.GetCurrentSessionAsync();

        if (session?.CustomerId != null)
        {
            context.Succeed(requirement);
        }
    }
}