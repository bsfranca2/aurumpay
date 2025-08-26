using Ardalis.Result;

using AurumPay.Core;

namespace AurumPay.Application.CheckoutSessions.Create;

public sealed record CreateCheckoutSessionCommand(Dictionary<string, int> CartItems) : ICommand<Result>;