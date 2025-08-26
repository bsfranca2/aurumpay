using Ardalis.Result;

using AurumPay.Application.Interfaces;
using AurumPay.Core;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Orders.Events;
using AurumPay.Domain.Outbox;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Methods;

namespace AurumPay.Application.Orders.ProcessOrderPayment;

internal sealed class ProcessOrderPaymentCommandHandler(
    IOrderRepository orderRepository,
    ICheckoutSessionRepository checkoutSessionRepository,
    IPaymentMethodRepository paymentMethodRepository,
    IOutboxRepository outboxRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ProcessOrderPaymentCommand, Result<OrderPaymentDto>>
{
    public async Task<Result<OrderPaymentDto>> Handle(
        ProcessOrderPaymentCommand request,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();

        Order? order = await orderRepository.GetWithPaymentsAsync(new OrderId(request.OrderId));
        if (order is null)
        {
            return Result.Error("Order not found");
        }

        if (!order.CanProcessPayment())
        {
            return Result.Error("Order cannot receive payment in current status");
        }

        CheckoutSession? checkoutSession = await checkoutSessionRepository.GetByOrderIdAsync(order.Id);
        if (checkoutSession?.SelectedPaymentMethodId is null || checkoutSession?.SelectedPaymentGatewayId is null)
        {
            return Result.Error("Payment method not selected");
        }

        PaymentMethod? paymentMethod = await paymentMethodRepository
            .GetActiveByIdAsync(checkoutSession.SelectedPaymentMethodId.Value);
        if (paymentMethod is null)
        {
            return Result.Error("Payment method not active");
        }

        OrderPaymentRequestedEvent paymentRequestedEvent = new(
            order.Id.Value,
            paymentMethod.Id.Value,
            request.PaymentData);
        OutboxMessage paymentRequestedMessage = OutboxMessage.Create(paymentRequestedEvent);
        await outboxRepository.AddAsync(paymentRequestedMessage);

        await unitOfWork.CommitTransactionAsync();

        return Result.Success();
    }
}