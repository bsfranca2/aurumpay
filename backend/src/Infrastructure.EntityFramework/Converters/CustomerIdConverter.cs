using AurumPay.Domain.Customers;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class CustomerIdConverter() : ValueConverter<CustomerId, long>(
    customerId => customerId.Value,
    value => new CustomerId(value)
);