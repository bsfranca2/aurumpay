using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Orders;

namespace AurumPay.Checkout.Presentation.Utilities;

public static class CheckoutSessionExtensions
{
    public static OrderId GetRequiredOrderId(this CheckoutSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        return session.OrderId ?? throw new InvalidOperationException("Checkout session has no order ID");
    }

    public static CustomerId GetRequiredCustomerId(this CheckoutSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        return session.CustomerId ?? throw new InvalidOperationException("Checkout session has no customer ID");
    }
}