using Ardalis.Result;

using AurumPay.Application.SeedWork;

namespace AurumPay.Application.CheckoutSessions.Create;

public sealed record CreateCheckoutSessionCommand(Dictionary<string, int> CartItems) : ICommand<Result>;