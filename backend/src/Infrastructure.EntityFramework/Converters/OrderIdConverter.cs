using AurumPay.Domain.Orders;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class OrderIdConverter() : ValueConverter<OrderId, long>(
    orderId => orderId.Value,
    value => new OrderId(value)
);
