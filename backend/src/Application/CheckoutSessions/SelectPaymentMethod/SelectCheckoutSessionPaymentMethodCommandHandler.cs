using Ardalis.Result;

using AurumPay.Core;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Configuration;
using AurumPay.Domain.Payments.Methods;

namespace AurumPay.Application.CheckoutSessions.SelectPaymentMethod;

public class SelectCheckoutSessionPaymentMethodCommandHandler(
    ICheckoutContext checkoutContext,
    ICheckoutSessionRepository checkoutSessionRepository,
    IPaymentMethodRepository paymentMethodRepository,
    IStorePaymentConfigurationService storePaymentConfigurationService
) : ICommandHandler<SelectCheckoutSessionPaymentMethodCommand, Result>
{
    public async Task<Result> Handle(SelectCheckoutSessionPaymentMethodCommand request, CancellationToken cancellationToken)
    {
        CheckoutSession? checkoutSession = await checkoutContext.SessionManager.GetCurrentSessionAsync();
        if (checkoutSession is null)
        {
            return Result.Error("No active checkout session found");
        }
        
        PaymentMethod? paymentMethod = await paymentMethodRepository.GetActiveByPaymentMethodTypeAsync(request.PaymentMethodType);

        if (paymentMethod is null)
        {
            return Result.Error("Payment method not available");
        }

        StorePaymentConfiguration? configuration = await storePaymentConfigurationService
            .GetActiveConfigurationAsync(checkoutSession.StoreId, paymentMethod.Id, cancellationToken);
        
        if (configuration is null)
        {
            return Result.Error("Payment method not active");
        }

        // TODO: Check if we can change payment method
        checkoutSession.SelectPaymentMethod(configuration.PaymentMethodId, configuration.PaymentGatewayId);

        await checkoutSessionRepository.UpdateAsync(checkoutSession);

        return Result.NoContent();
    }
}