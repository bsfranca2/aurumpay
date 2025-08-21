namespace AurumPay.Checkout.Presentation.Contracts;

public record CreateCheckoutRequest(Dictionary<string, int> CartItems);