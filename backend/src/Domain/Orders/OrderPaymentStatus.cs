namespace AurumPay.Domain.Orders;

public enum OrderPaymentStatus
{
    Pending = 0,
    PartiallyPaid = 1,
    Paid = 2,
    PaymentFailed = 3,
    Refunded = 4
}