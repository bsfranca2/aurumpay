using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Payments.Transactions;

namespace AurumPay.Application.Orders.ProcessOrderPayment;

public sealed record ProcessOrderPaymentCommand(
    long OrderId,
    Dictionary<string, object> PaymentData
) : ICommand<Result<OrderPaymentDto>>;

public record OrderPaymentDto(
    long OrderId,
    string PaymentId,
    PaymentStatus PaymentStatus,
    OrderStatus OrderStatus,
    OrderPaymentStatus OrderPaymentStatus,
    decimal Amount,
    string? PaymentInstructions
);