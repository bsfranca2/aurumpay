using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.SeedWork;

namespace AurumPay.Application.CheckoutSessions.UpdateCustomerAddress;

public sealed record UpdateCheckoutSessionCustomerAddressCommand(
    long AddressId,
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string City,
    string State
) : ICommand<Result<CustomerAddressDto>>;