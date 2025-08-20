using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Interfaces;

/// <summary>
/// High-level service for payment processing
/// </summary>
public interface IPaymentProcessingService
{
    Task<Payment> ProcessPaymentAsync(
        PaymentRequest request,
        StoreId storeId,
        PaymentMethodId paymentMethodId,
        CancellationToken cancellationToken = default);

    Task<PaymentValidationResult> ValidatePaymentAsync(
        PaymentId paymentId,
        CancellationToken cancellationToken = default);
    
    Task<RefundResult> RefundPaymentAsync(
        PaymentId paymentId,
        decimal? amount = null,
        CancellationToken cancellationToken = default);
}