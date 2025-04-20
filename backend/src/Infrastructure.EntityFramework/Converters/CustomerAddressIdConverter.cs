using AurumPay.Domain.Customers;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class CustomerAddressIdConverter() : ValueConverter<CustomerAddressId, long>(
    customerAddressId => customerAddressId.Value,
    value => new CustomerAddressId(value)
);