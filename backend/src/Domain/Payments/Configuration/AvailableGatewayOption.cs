using AurumPay.Domain.Payments.Gateways;

namespace AurumPay.Domain.Payments.Configuration;

public record AvailableGatewayOption(
    PaymentGatewayId GatewayId,
    string GatewayName,
    string DisplayName,
    bool IsActive,
    bool IsConfigured,
    decimal? ProcessingFee,
    string? LogoUrl
);