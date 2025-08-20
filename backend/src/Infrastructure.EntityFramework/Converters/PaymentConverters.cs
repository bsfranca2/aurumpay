using AurumPay.Domain.Payments.Configuration;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class PaymentMethodIdConverter() : ValueConverter<PaymentMethodId, long>(
    id => id.Value,
    value => new PaymentMethodId(value)
);

public class PaymentGatewayIdConverter() : ValueConverter<PaymentGatewayId, long>(
    id => id.Value,
    value => new PaymentGatewayId(value)
);

public class StorePaymentConfigurationIdConverter() : ValueConverter<StorePaymentConfigurationId, long>(
    id => id.Value,
    value => new StorePaymentConfigurationId(value)
);

public class PaymentIdConverter() : ValueConverter<PaymentId, Guid>(
    id => id.Value.ToGuid(),
    value => new PaymentId(new Ulid(value))
);