using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Transactions;

namespace AurumPay.Domain.Interfaces;

/// <summary>
/// Contract for payment gateway implementations
/// </summary>
public interface IPaymentGatewayService
{
    PaymentGatewayType GatewayType { get; }
    
    Task<PaymentProcessingResult> ProcessPaymentAsync(
        PaymentRequest request, 
        PaymentGatewayCredentials credentials,
        CancellationToken cancellationToken = default);
    
    Task<PaymentValidationResult> ValidatePaymentAsync(
        string gatewayTransactionId, 
        PaymentGatewayCredentials credentials,
        CancellationToken cancellationToken = default);
    
    Task<RefundResult> RefundPaymentAsync(
        string gatewayTransactionId, 
        decimal amount, 
        PaymentGatewayCredentials credentials,
        CancellationToken cancellationToken = default);
}