using AurumPay.Domain.Orders;
using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.CheckoutSessions;

public interface ICheckoutSessionRepository : IRepository<CheckoutSession, CheckoutSessionId>
{
    Task<CheckoutSession?> GetByOrderIdAsync(OrderId orderId);
}