using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class StoreIdConverter() : ValueConverter<StoreId, long>(
    storeId => storeId.Value,
    value => new StoreId((int)value)
);