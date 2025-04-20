using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.Customers;

namespace AurumPay.Application.Customers.GetAddresses;

public sealed record GetCustomerAddressesQuery(CustomerId CustomerId) : IQuery<Result<List<CustomerAddressDto>>>;