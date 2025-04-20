using AurumPay.Domain.Shared;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class CepConverter() : ValueConverter<Cep, string>(
    cep => cep.Value,
    value => new Cep(value)
);