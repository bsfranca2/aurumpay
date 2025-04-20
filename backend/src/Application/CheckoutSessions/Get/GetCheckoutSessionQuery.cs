using Ardalis.Result;

using AurumPay.Application.SeedWork;

namespace AurumPay.Application.CheckoutSessions.Get;

public record GetCheckoutSessionQuery : IQuery<Result<CheckoutSessionDto>>;

public record CheckoutSessionDto(
    IEnumerable<CartItemDto> cartItems
);
