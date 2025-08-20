namespace AurumPay.Checkout.Presentation.Checkouts;

public record SelectPaymentMethodDto
{
    public string PaymentMethodType { get; set; } = string.Empty;
}