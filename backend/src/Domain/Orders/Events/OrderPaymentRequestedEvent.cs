using AurumPay.Core;

namespace AurumPay.Domain.Orders.Events;

public record OrderPaymentRequestedEvent(
    long OrderId,
    long PaymentMethodId,
    Dictionary<string, object> PaymentData
) : BaseEvent;