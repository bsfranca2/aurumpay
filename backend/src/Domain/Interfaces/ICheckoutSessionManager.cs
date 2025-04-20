using AurumPay.Domain.CheckoutSessions;

namespace AurumPay.Domain.Interfaces;

public interface ICheckoutSessionManager
{
    CheckoutSessionId? GetCurrentSessionId();
    Task<CheckoutSession?> GetCurrentSessionAsync();
    Task EstablishSessionAsync(CheckoutSession checkoutSession);
    Task EndSessionAsync();
}
