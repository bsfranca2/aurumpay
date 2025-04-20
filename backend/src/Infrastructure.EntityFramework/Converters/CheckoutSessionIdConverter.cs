using AurumPay.Domain.CheckoutSessions;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class CheckoutSessionIdConverter() : ValueConverter<CheckoutSessionId, Guid>(
    checkoutSessionId => checkoutSessionId.Value.ToGuid(),
    value => new CheckoutSessionId(new Ulid(value))
);