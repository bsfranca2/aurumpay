using AurumPay.Core;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;

namespace AurumPay.Domain.Orders;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<IEnumerable<Order>> GetByCustomerAsync(CustomerId customerId);
    Task<Order?> GetWithPaymentsAsync(OrderId orderId);
}