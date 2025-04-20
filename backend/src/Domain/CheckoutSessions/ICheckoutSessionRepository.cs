using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.CheckoutSessions;

public interface ICheckoutSessionRepository : IRepository<CheckoutSession, CheckoutSessionId>
{
}