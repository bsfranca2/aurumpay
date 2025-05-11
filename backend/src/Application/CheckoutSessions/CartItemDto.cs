namespace AurumPay.Application.CheckoutSessions;

public record CartItemDto(
    CartItemProductDto Product,
    int Quantity
);