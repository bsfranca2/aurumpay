using AurumPay.Domain.Shared;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class EmailAddressConverter() : ValueConverter<EmailAddress, string>(
    email => email.Value,
    value => new EmailAddress(value)
);