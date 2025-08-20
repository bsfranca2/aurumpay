using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Orders;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class OrderRepository(DatabaseContext context)
    : Repository<Order, OrderId, DatabaseContext>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetByCustomerAsync(CustomerId customerId)
    {
        return await DbSet
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Order?> GetWithPaymentsAsync(OrderId orderId)
    {
        return await DbSet
            .Include(o => o.Payments)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}