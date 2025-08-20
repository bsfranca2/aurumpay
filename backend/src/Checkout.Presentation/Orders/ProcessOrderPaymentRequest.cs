namespace AurumPay.Checkout.Presentation.Orders;

public record ProcessOrderPaymentRequest(
    Dictionary<string, object> PaymentData
);