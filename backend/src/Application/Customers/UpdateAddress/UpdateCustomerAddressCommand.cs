using Ardalis.Result;

using AurumPay.Application.SeedWork;

namespace AurumPay.Application.Customers.UpdateAddress;

public sealed record UpdateCustomerAddressCommand(
    long CustomerId,
    long AddressId,
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string City,
    string State,
    bool IsMain
) : ICommand<Result<CustomerAddressDto>>;