using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.SeedWork;

namespace AurumPay.Application.CheckoutSessions.AddCustomerAddress;

public sealed record AddCheckoutSessionCustomerAddressCommand(
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string City,
    string State
) : ICommand<Result<CustomerAddressDto>>;