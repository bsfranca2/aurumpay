using Ardalis.Result;

using AurumPay.Core;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;

namespace AurumPay.Application.CheckoutSessions.Get;

public record GetCheckoutSessionQuery : IQuery<Result<CheckoutSessionDto>>;

public record CheckoutSessionDto(
    CheckoutStatus Status,
    IEnumerable<CartItemDto> CartItems,
    CustomerDto? Customer = null,
    PaymentMethodId? PaymentMethodId = null,
    PaymentGatewayId? PaymentGatewayId = null
);