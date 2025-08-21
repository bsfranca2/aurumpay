namespace AurumPay.Checkout.Presentation.Contracts;

public record ProcessOrderPaymentRequest(
    Dictionary<string, object> PaymentData
);