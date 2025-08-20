namespace AurumPay.Domain.CheckoutSessions;

public enum CheckoutStatus
{
    Pending = 0,
    CustomerIdentified = 1,
    PaymentMethodSelected = 2,
    OrderCreated = 3,
    PaymentProcessing = 4,
    PaymentCompleted = 5,
    PaymentFailed = 6,
    Completed = 7
}