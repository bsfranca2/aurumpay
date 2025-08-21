namespace AurumPay.Checkout.Presentation.Contracts;

public record SelectPaymentMethodRequest
{
    public string PaymentMethodType { get; set; } = string.Empty;
}