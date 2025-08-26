using Ardalis.Result;

using AurumPay.Core;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Payments.Transactions;

namespace AurumPay.Application.CheckoutSessions.Finalize;

public sealed record FinalizeCheckoutSessionCommand : ICommand<Result<OrderDto>>;

public record OrderDto(
    long Id,
    long CustomerId,
    long StoreId,
    decimal OrderTotal,
    OrderStatus Status,
    OrderPaymentStatus PaymentStatus,
    DateTime CreatedAt,
    List<OrderItemDto> Items
);

public record OrderItemDto(
    long ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);