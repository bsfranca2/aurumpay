using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

namespace AurumPay.Checkout.Presentation.Utilities;

public static class CheckoutSessionManagerExtensions
{
    public static async Task<CheckoutSession> GetRequiredSessionAsync(this ICheckoutSessionManager sessionManager)
    {
        ArgumentNullException.ThrowIfNull(sessionManager);
        CheckoutSession? checkoutSession = await sessionManager.GetCurrentSessionAsync();
        return checkoutSession ?? throw new InvalidOperationException("No active checkout session found");
    }
}