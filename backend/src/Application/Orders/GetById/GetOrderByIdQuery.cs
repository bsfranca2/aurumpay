using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Finalize;
using AurumPay.Application.SeedWork;

namespace AurumPay.Application.Orders.GetById;

public sealed record GetOrderByIdQuery(long OrderId) : IQuery<Result<OrderDto>>;
