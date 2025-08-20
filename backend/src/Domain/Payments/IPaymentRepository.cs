using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Payments.Transactions;
using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.Payments;

public interface IPaymentRepository : IRepository<Payment, PaymentId>
{
    // Task<IEnumerable<Payment>> GetByCheckoutSessionAsync(CheckoutSessionId checkoutSessionId);
    Task<IEnumerable<Payment>> GetByCustomerAsync(CustomerId customerId);
    Task<Payment?> GetByGatewayTransactionIdAsync(string gatewayTransactionId);
}