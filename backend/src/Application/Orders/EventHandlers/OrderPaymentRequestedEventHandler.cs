using AurumPay.Application.Interfaces;
using AurumPay.Core;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Orders.Events;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Exceptions;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;

using Microsoft.Extensions.Logging;

namespace AurumPay.Application.Orders.EventHandlers;

public class OrderPaymentRequestedEventHandler(
    ILogger<OrderPaymentRequestedEventHandler> logger,
    IUnitOfWork unitOfWork,
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IPaymentMethodRepository paymentMethodRepository,
    ICheckoutSessionRepository checkoutSessionRepository,
    IPaymentProcessingService paymentProcessingService
) : IEventHandler<OrderPaymentRequestedEvent>
{
    public async Task HandleAsync(OrderPaymentRequestedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing payment for Order {OrderId}", @event.OrderId);

        await unitOfWork.BeginTransactionAsync();

        Order? order = await orderRepository.GetWithPaymentsAsync(new OrderId(@event.OrderId));

        PaymentMethod? paymentMethod = await paymentMethodRepository
            .GetActiveByIdAsync(new PaymentMethodId(@event.PaymentMethodId));

        // TODO: What should I do?
        if (order is null || paymentMethod is null)
        {
            return;
        }

        CheckoutSession? checkoutSession = await checkoutSessionRepository.GetByOrderIdAsync(order.Id);

        Customer? customer = await customerRepository.GetByIdAsync(order.CustomerId);

        // TODO: What should I do?
        if (checkoutSession is null || customer is null)
        {
            return;
        }

        PaymentRequest paymentRequest = new(
            order.OrderTotal - order.PaidAmount, // Remaining amount
            paymentMethod.Type,
            @event.PaymentData,
            customer,
            $"Payment for Order {order.Id.Value}",
            order.Id.Value.ToString()
        );

        try
        {
            Payment payment = await paymentProcessingService.ProcessPaymentAsync(
                paymentRequest,
                order.StoreId,
                paymentMethod.Id,
                cancellationToken
            );

            order.AddPayment(payment);

            if (payment.Status == PaymentStatus.Completed)
            {
                order.ConfirmPayment();

                checkoutSession.MarkPaymentCompleted();
            }
            else if (payment.Status == PaymentStatus.Failed)
            {
                checkoutSession.MarkPaymentFailed();
            }
            else
            {
                checkoutSession.MarkPaymentProcessing();
            }

            await checkoutSessionRepository.UpdateAsync(checkoutSession);

            await orderRepository.UpdateAsync(order);
        }
        catch (PaymentProcessingException)
        {
            // TODO: What should I do?
        }

        await unitOfWork.CommitTransactionAsync();
    }
}