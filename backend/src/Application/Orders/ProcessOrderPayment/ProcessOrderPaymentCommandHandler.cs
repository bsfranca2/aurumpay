using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Exceptions;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;

namespace AurumPay.Application.Orders.ProcessOrderPayment;

internal sealed class ProcessOrderPaymentCommandHandler(
    IOrderRepository orderRepository,
    IPaymentProcessingService paymentProcessingService,
    ICheckoutSessionRepository checkoutSessionRepository,
    ICustomerRepository customerRepository,
    IPaymentMethodRepository paymentMethodRepository
) : ICommandHandler<ProcessOrderPaymentCommand, Result<OrderPaymentDto>>
{
    public async Task<Result<OrderPaymentDto>> Handle(
        ProcessOrderPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetWithPaymentsAsync(new OrderId(request.OrderId));
        if (order is null)
        {
            return Result.Error("Order not found");
        }
        
        if (!order.CanProcessPayment())
        {
            return Result.Error("Order cannot receive payment in current status");
        }

        var checkoutSession = await checkoutSessionRepository.GetByOrderIdAsync(order.Id);
        if (checkoutSession?.SelectedPaymentMethodId is null || checkoutSession?.SelectedPaymentGatewayId is null)
        {
            return Result.Error("Payment method not selected");
        }

        var paymentMethod = await paymentMethodRepository.GetByIdAsync(checkoutSession.SelectedPaymentMethodId.Value);
        if (paymentMethod is null || !paymentMethod.IsActive)
        {
            return Result.Error("Payment method not active");
        }

        var paymentRequest = new PaymentRequest(
            order.OrderTotal - order.PaidAmount, // Remaining amount
            paymentMethod.Type,
            request.PaymentData,
            await GetCustomer(order.CustomerId),
            $"Payment for Order {order.Id.Value}",
            order.Id.Value.ToString()
        );

        try
        {
            var payment = await paymentProcessingService.ProcessPaymentAsync(
                paymentRequest,
                order.StoreId,
                paymentMethod.Id,
                cancellationToken
            );

            // Link payment to order
            // payment.LinkToOrder(order.Id);
            
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

            var paymentInstructions = ExtractPaymentInstructions(payment, request.PaymentData);

            return Result.Success(new OrderPaymentDto(
                order.Id.Value,
                payment.Id.Value.ToString(),
                payment.Status,
                order.Status,
                order.PaymentStatus,
                payment.Amount,
                paymentInstructions
            ));
        }
        catch (PaymentProcessingException ex)
        {
            return Result.Error(ex.Message);
        }
    }

    private async Task<Customer?> GetCustomer(CustomerId customerId)
    {
        return await customerRepository.GetByIdAsync(customerId);
    }

    private static string? ExtractPaymentInstructions(Payment payment, Dictionary<string, object> paymentData)
    {
        return payment.Status switch
        {
            PaymentStatus.Pending when paymentData.ContainsKey("qr_code") => 
                paymentData["qr_code"].ToString(),
            PaymentStatus.Pending when paymentData.ContainsKey("boleto_url") => 
                paymentData["boleto_url"].ToString(),
            PaymentStatus.Pending when paymentData.ContainsKey("payment_link") => 
                paymentData["payment_link"].ToString(),
            _ => null
        };
    }
}