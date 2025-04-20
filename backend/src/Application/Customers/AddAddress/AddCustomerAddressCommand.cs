using Ardalis.Result;

using AurumPay.Application.SeedWork;

namespace AurumPay.Application.Customers.AddAddress;

public sealed record AddCustomerAddressCommand(
    long CustomerId,
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string City,
    string State,
    bool IsMain
) : ICommand<Result<CustomerAddressDto>>;