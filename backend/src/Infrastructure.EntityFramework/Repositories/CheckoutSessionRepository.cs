using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Orders;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class CheckoutSessionRepository(DatabaseContext context)
    : Repository<CheckoutSession, CheckoutSessionId, DatabaseContext>(context), ICheckoutSessionRepository
{
    public Task<CheckoutSession?> GetByOrderIdAsync(OrderId orderId)
    {
        return DbSet.FirstOrDefaultAsync(cs => cs.OrderId == orderId);
    }
}