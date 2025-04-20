using Ardalis.Result;

using AurumPay.Application.SeedWork;

namespace AurumPay.Application.CheckoutSessions.UpdateCustomer;

public sealed record UpdateCheckoutSessionCustomerCommand(
    string FullName,
    string Email,
    string PhoneNumber,
    string Cpf
) : ICommand<Result>;