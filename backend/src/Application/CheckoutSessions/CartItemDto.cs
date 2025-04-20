namespace AurumPay.Application.CheckoutSessions;

public record CartItemDto(
    long ProductId,
    int Quantity
);