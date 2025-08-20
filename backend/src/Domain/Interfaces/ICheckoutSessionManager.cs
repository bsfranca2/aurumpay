using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;

namespace AurumPay.Domain.Interfaces;

public interface ICheckoutSessionManager
{
    CheckoutSessionId? GetCurrentSessionId();
    Task<CheckoutSession?> GetCurrentSessionAsync();
    Task<Customer?> GetCurrentCustomerAsync();
    
    Task EstablishSessionAsync(CheckoutSession checkoutSession);
    Task EndSessionAsync();
}
