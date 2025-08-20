using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.Payments.Methods;

namespace AurumPay.Application.CheckoutSessions.SelectPaymentMethod;

public record SelectCheckoutSessionPaymentMethodCommand(
    PaymentMethodType PaymentMethodType
) : ICommand<Result>;