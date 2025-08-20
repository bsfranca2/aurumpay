using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Configuration;
using AurumPay.Domain.Payments.Exceptions;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;
using AurumPay.Domain.Stores;

namespace AurumPay.Infrastructure.Services;

public class PaymentProcessingService(
    IStorePaymentConfigurationService storePaymentConfigurationService,
    IEnumerable<IPaymentGatewayService> paymentGatewayServices,
    IPaymentRepository paymentRepository,
    IPaymentGatewayRepository paymentGatewayRepository
) : IPaymentProcessingService
{
    private readonly Dictionary<PaymentGatewayType, IPaymentGatewayService> _gatewayServices = 
        paymentGatewayServices.ToDictionary(x => x.GatewayType);

    public async Task<Payment> ProcessPaymentAsync(
        PaymentRequest request,
        StoreId storeId,
        PaymentMethodId paymentMethodId,
        CancellationToken cancellationToken = default)
    {
        var activeConfig = await storePaymentConfigurationService
            .GetActiveConfigurationAsync(storeId, paymentMethodId, cancellationToken);
        
        if (activeConfig is null)
        {
            throw new PaymentProcessingException("No active payment gateway configured for this payment method");
        }

        return await ProcessWithGateway(request, activeConfig, cancellationToken);
    }

    private async Task<Payment> ProcessWithGateway(
        PaymentRequest request,
        StorePaymentConfiguration config,
        CancellationToken cancellationToken)
    {
        var gateway = await GetGatewayDetails(config.PaymentGatewayId);
        if (gateway is null)
        {
            throw new PaymentProcessingException($"Gateway {config.PaymentGatewayId} not found");
        }

        if (!_gatewayServices.TryGetValue(gateway.Type, out var gatewayService))
        {
            throw new PaymentProcessingException($"No service implementation for gateway type {gateway.Type}");
        }
        
        var payment = Payment.Create(
            new OrderId(Convert.ToInt64(request.ExternalReference)),
            request.Customer!.Id, // TODO: Ensure value exists
            config.PaymentMethodId,
            config.PaymentGatewayId,
            request.Amount
        );

        await paymentRepository.CreateAsync(payment);

        try
        {
            var result = await gatewayService.ProcessPaymentAsync(request, config.Credentials, cancellationToken);

            if (result.IsSuccess)
            {
                payment.MarkAsProcessing(result.TransactionId!);
                
                if (result.Status == PaymentStatus.Completed)
                {
                    payment.MarkAsCompleted(System.Text.Json.JsonSerializer.Serialize(result.GatewayResponse));
                }
                
                await paymentRepository.UpdateAsync(payment);
            }
            else
            {
                payment.MarkAsFailed(result.ErrorMessage!, System.Text.Json.JsonSerializer.Serialize(result.GatewayResponse));
                await paymentRepository.UpdateAsync(payment);
                throw new PaymentProcessingException(result.ErrorMessage!);
            }
            
            return payment;
        }
        catch (Exception ex) when (!(ex is PaymentProcessingException))
        {
            payment.MarkAsFailed($"Gateway processing error: {ex.Message}");
            await paymentRepository.UpdateAsync(payment);
            throw new PaymentProcessingException($"Gateway processing failed: {ex.Message}", ex);
        }
    }

    public async Task<PaymentValidationResult> ValidatePaymentAsync(
        PaymentId paymentId,
        CancellationToken cancellationToken = default)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment is null)
        {
            return new PaymentValidationResult(false, PaymentStatus.Failed, null, "Payment not found");
        }

        var config = await storePaymentConfigurationService.GetConfigurationAsync(
            new StoreId(), // TODO: payment.CheckoutSessionId.Value
            payment.PaymentMethodId,
            payment.PaymentGatewayId,
            cancellationToken);

        if (config is null)
        {
            return new PaymentValidationResult(false, PaymentStatus.Failed, null, "Payment configuration not found");
        }

        var gateway = await GetGatewayDetails(payment.PaymentGatewayId);
        if (gateway is null || !_gatewayServices.TryGetValue(gateway.Type, out var gatewayService))
        {
            return new PaymentValidationResult(false, PaymentStatus.Failed, null, "Gateway service not available");
        }

        return await gatewayService.ValidatePaymentAsync(
            payment.GatewayTransactionId!,
            config.Credentials,
            cancellationToken);
    }

    public async Task<RefundResult> RefundPaymentAsync(
        PaymentId paymentId,
        decimal? amount = null,
        CancellationToken cancellationToken = default)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment is null)
        {
            return new RefundResult(false, null, "Payment not found");
        }

        if (payment.Status != PaymentStatus.Completed)
        {
            return new RefundResult(false, null, "Only completed payments can be refunded");
        }

        var refundAmount = amount ?? payment.Amount;
        if (refundAmount > payment.Amount)
        {
            return new RefundResult(false, null, "Refund amount cannot exceed payment amount");
        }

        var config = await storePaymentConfigurationService.GetConfigurationAsync(
            new StoreId(), // TODO: payment.CheckoutSessionId.Value,
            payment.PaymentMethodId,
            payment.PaymentGatewayId,
            cancellationToken);

        if (config is null)
        {
            return new RefundResult(false, null, "Payment configuration not found");
        }

        var gateway = await GetGatewayDetails(payment.PaymentGatewayId);
        if (gateway is null || !_gatewayServices.TryGetValue(gateway.Type, out var gatewayService))
        {
            return new RefundResult(false, null, "Gateway service not available");
        }

        var result = await gatewayService.RefundPaymentAsync(
            payment.GatewayTransactionId!,
            refundAmount,
            config.Credentials,
            cancellationToken);

        if (result.IsSuccess)
        {
            payment.MarkAsRefunded();
            await paymentRepository.UpdateAsync(payment);
        }

        return result;
    }
    
    private async Task<PaymentGateway?> GetGatewayDetails(PaymentGatewayId gatewayId)
    {
        return await paymentGatewayRepository.GetByIdAsync(gatewayId);
    }
}