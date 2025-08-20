using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Transactions;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class PaymentRepository(DatabaseContext context)
    : Repository<Payment, PaymentId, DatabaseContext>(context), IPaymentRepository
{
    // public async Task<IEnumerable<Payment>> GetByCheckoutSessionAsync(CheckoutSessionId checkoutSessionId)
    // {
    //     return await DbSet
    //         .Where(p => p.CheckoutSessionId == checkoutSessionId)
    //         .OrderBy(p => p.CreatedAt)
    //         .ToListAsync();
    // }

    public async Task<IEnumerable<Payment>> GetByCustomerAsync(CustomerId customerId)
    {
        return await DbSet
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Payment?> GetByGatewayTransactionIdAsync(string gatewayTransactionId)
    {
        return await DbSet
            .FirstOrDefaultAsync(p => p.GatewayTransactionId == gatewayTransactionId);
    }
}