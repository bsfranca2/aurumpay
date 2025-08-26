using Ardalis.Result;

using AurumPay.Core;

namespace AurumPay.Application.Customers.UpdateAddress;

public sealed record UpdateCustomerAddressCommand(
    long CustomerId,
    long AddressId,
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Recipient,
    bool IsMain
) : ICommand<Result<CustomerAddressDto>>;