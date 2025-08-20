using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Payments.Transactions;
using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Orders;

public class Order : IEntity<OrderId>
{
    private readonly HashSet<OrderItem> _orderItems = [];
    private readonly HashSet<Payment> _payments = [];

    public OrderId Id { get; }
    public StoreId StoreId { get; }
    public CustomerId CustomerId { get; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.ToList();
    public IReadOnlyCollection<Payment> Payments => _payments.ToList();

    public decimal OrderTotal { get; }
    public decimal PaidAmount { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? PaidDateUtc { get; private set; }

    public OrderStatus Status { get; private set; }
    public OrderPaymentStatus PaymentStatus { get; private set; }

    private Order(
        OrderId id,
        StoreId storeId,
        CustomerId customerId,
        decimal orderTotal,
        decimal paidAmount,
        DateTime createdAtUtc,
        DateTime? paidDateUtc,
        OrderStatus status,
        OrderPaymentStatus paymentStatus)
    {
        Id = id;
        StoreId = storeId;
        CustomerId = customerId;
        OrderTotal = orderTotal;
        PaidAmount = paidAmount;
        CreatedAtUtc = createdAtUtc;
        PaidDateUtc = paidDateUtc;
        Status = status;
        PaymentStatus = paymentStatus;
    }

    public static Order Create(StoreId storeId, CustomerId customerId, CheckoutSessionId checkoutSessionId,
        IEnumerable<OrderItem> orderItems)
    {
        List<OrderItem> orderItemsList = orderItems.ToList();
        decimal orderTotal = orderItemsList.Sum(orderItem => orderItem.UnitPrice);

        Order order = new(new OrderId(), storeId, customerId, orderTotal, 0, DateTime.UtcNow, null, OrderStatus.Pending,
            OrderPaymentStatus.Pending);

        foreach (OrderItem orderItem in orderItemsList)
        {
            order._orderItems.Add(orderItem);
        }

        return order;
    }

    public void AddPayment(Payment payment)
    {
        _payments.Add(payment);
        UpdatePaymentStatus();
    }

    public void ConfirmPayment()
    {
        if (PaymentStatus == OrderPaymentStatus.Paid)
        {
            Status = OrderStatus.Confirmed;
            PaidDateUtc = DateTime.UtcNow;
        }
    }

    public void MarkAsProcessing()
    {
        if (Status == OrderStatus.Confirmed)
        {
            Status = OrderStatus.Processing;
        }
    }

    public void MarkAsShipped()
    {
        if (Status == OrderStatus.Processing)
        {
            Status = OrderStatus.Shipped;
        }
    }

    public void MarkAsDelivered()
    {
        if (Status == OrderStatus.Shipped)
        {
            Status = OrderStatus.Delivered;
        }
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Pending or OrderStatus.Confirmed)
        {
            Status = OrderStatus.Cancelled;
        }
    }

    public void MarkAsRefunded()
    {
        Status = OrderStatus.Refunded;
        PaymentStatus = OrderPaymentStatus.Refunded;
    }

    private void UpdatePaymentStatus()
    {
        IEnumerable<Payment> completedPayments =
            _payments.Where(p => p.Status == Domain.Payments.Transactions.PaymentStatus.Completed);
        decimal totalPaid = completedPayments.Sum(p => p.Amount);

        PaidAmount = totalPaid;

        PaymentStatus = totalPaid switch
        {
            0 => OrderPaymentStatus.Pending,
            var paid when paid >= OrderTotal => OrderPaymentStatus.Paid,
            _ => OrderPaymentStatus.PartiallyPaid
        };

        if (PaymentStatus == OrderPaymentStatus.Paid && PaidDateUtc == null)
        {
            PaidDateUtc = DateTime.UtcNow;
        }
    }

    public bool CanProcessPayment()
    {
        return Status == OrderStatus.Pending && PaymentStatus != OrderPaymentStatus.Paid;
    }

    public bool HasSuccessfulPayment()
    {
        return _payments.Any(p => p.Status == Domain.Payments.Transactions.PaymentStatus.Completed);
    }
}