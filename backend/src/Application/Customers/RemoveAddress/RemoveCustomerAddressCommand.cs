using Ardalis.Result;

using AurumPay.Core;

namespace AurumPay.Application.Customers.RemoveAddress;

public sealed record RemoveCustomerAddressCommand(long CustomerId, long AddressId) : ICommand<Result>;