using AurumPay.Core;
using AurumPay.Domain.Orders;

namespace AurumPay.Domain.CheckoutSessions;

public interface ICheckoutSessionRepository : IRepository<CheckoutSession, CheckoutSessionId>
{
    Task<CheckoutSession?> GetByOrderIdAsync(OrderId orderId);
}