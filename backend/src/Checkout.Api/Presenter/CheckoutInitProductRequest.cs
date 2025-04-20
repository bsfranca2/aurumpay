using AurumPay.Application.CheckoutSessions;

namespace AurumPay.Checkout.Api.Presenter;

public record CheckoutInitProductRequest(List<CartItemDto> Items);