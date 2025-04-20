using AurumPay.Domain.Shared;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class TelephoneConverter() : ValueConverter<Telephone, string>(
    telephone => telephone.Value,
    value => new Telephone(value)
);