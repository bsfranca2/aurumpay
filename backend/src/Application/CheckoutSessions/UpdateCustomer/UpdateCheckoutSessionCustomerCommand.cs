using Ardalis.Result;

using AurumPay.Core;

namespace AurumPay.Application.CheckoutSessions.UpdateCustomer;

public sealed record UpdateCheckoutSessionCustomerCommand(
    string FullName,
    string Email,
    string PhoneNumber,
    string Cpf
) : ICommand<Result>;