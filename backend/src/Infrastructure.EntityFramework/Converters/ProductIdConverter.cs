using AurumPay.Domain.Catalog;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class ProductIdConverter() : ValueConverter<ProductId, long>(
    productId => productId.Value,
    value => new ProductId(value)
);