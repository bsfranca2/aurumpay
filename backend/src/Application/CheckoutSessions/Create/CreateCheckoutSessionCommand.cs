using Ardalis.Result;

using AurumPay.Application.SeedWork;

namespace AurumPay.Application.CheckoutSessions.Create;

public sealed record CreateCheckoutSessionCommand(List<CartItemDto> CartItems) : ICommand<Result>;