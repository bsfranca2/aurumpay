using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.Orders;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<IEnumerable<Order>> GetByCustomerAsync(CustomerId customerId);
    Task<Order?> GetWithPaymentsAsync(OrderId orderId);
}