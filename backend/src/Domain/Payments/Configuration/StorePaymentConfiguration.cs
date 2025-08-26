using AurumPay.Core;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Payments.Configuration;

/// <summary>
///     Configuration of a payment method for a specific store using a specific gateway
/// </summary>
public sealed class StorePaymentConfiguration : IEntity<StorePaymentConfigurationId>
{
    public StorePaymentConfigurationId Id { get; }
    public StoreId StoreId { get; }
    public PaymentMethodId PaymentMethodId { get; }
    public PaymentGatewayId PaymentGatewayId { get; }
    public bool IsEnabled { get; private set; }
    public bool IsActive { get; private set; }
    public PaymentGatewayCredentials Credentials { get; private set; }

    // EF
    private StorePaymentConfiguration()
    {
        Credentials = new PaymentGatewayCredentials(string.Empty, string.Empty);
    }

    private StorePaymentConfiguration(
        StorePaymentConfigurationId id,
        StoreId storeId,
        PaymentMethodId paymentMethodId,
        PaymentGatewayId paymentGatewayId,
        PaymentGatewayCredentials credentials)
    {
        Id = id;
        StoreId = storeId;
        PaymentMethodId = paymentMethodId;
        PaymentGatewayId = paymentGatewayId;
        Credentials = credentials;
        IsEnabled = true;
    }

    public static StorePaymentConfiguration Create(
        StoreId storeId,
        PaymentMethodId paymentMethodId,
        PaymentGatewayId paymentGatewayId,
        PaymentGatewayCredentials credentials)
    {
        return new StorePaymentConfiguration(
            new StorePaymentConfigurationId(),
            storeId,
            paymentMethodId,
            paymentGatewayId,
            credentials);
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateCredentials(PaymentGatewayCredentials newCredentials)
    {
        Credentials = newCredentials;
    }
}