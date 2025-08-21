using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Finalize;
using AurumPay.Application.CheckoutSessions.Get;
using AurumPay.Application.CheckoutSessions.SelectPaymentMethod;
using AurumPay.Application.CheckoutSessions.UpdateCustomer;
using AurumPay.Application.Data;
using AurumPay.Application.Orders.ProcessOrderPayment;
using AurumPay.Checkout.Presentation.Contracts;
using AurumPay.Checkout.Presentation.Utilities;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Payments.Methods;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.Endpoints;

public class CheckoutSessionEndpoints : CarterModule
{
    public CheckoutSessionEndpoints() : base("/checkout")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/summary", CheckoutSummary)
            .WithName(nameof(CheckoutSummary))
            .Produces<CheckoutSessionDto>();

        app.MapPut("/customer", CheckoutCustomer)
            .WithName(nameof(CheckoutCustomer))
            .ProducesValidationProblem();

        app.MapPut("/payment-method", SelectCheckoutPaymentMethod)
            .WithName(nameof(SelectCheckoutPaymentMethod))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        app.MapPost("/finalize", FinalizeCheckoutSession)
            .WithName(nameof(FinalizeCheckoutSession))
            .Produces<OrderDto>()
            .ProducesValidationProblem();

        app.MapPost("/payment", ProcessOrderPayment)
            .WithName(nameof(ProcessOrderPayment))
            .RequireAuthorization(Policies.CheckoutSessionOrder)
            .Produces<OrderPaymentDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CheckoutSummary(ISender sender)
    {
        Result<CheckoutSessionDto> result = await sender.Send(new GetCheckoutSessionQuery());
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> CheckoutCustomer(IdentifyCustomerRequest request, ISender sender)
    {
        UpdateCheckoutSessionCustomerCommand command = new(request.FullName, request.Email, request.PhoneNumber, request.Cpf);
        Result result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> SelectCheckoutPaymentMethod(SelectPaymentMethodRequest request, ISender sender)
    {
        if (Enum.TryParse(request.PaymentMethodType, out PaymentMethodType paymentMethodType))
        {
            SelectCheckoutSessionPaymentMethodCommand command = new(paymentMethodType);
            Result result = await sender.Send(command);
            return result.ToMinimalApiResult();
        }

        return Results.BadRequest();
    }

    private static async Task<IResult> FinalizeCheckoutSession(ISender sender)
    {
        FinalizeCheckoutSessionCommand sessionCommand = new();
        Result<OrderDto> result = await sender.Send(sessionCommand);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> ProcessOrderPayment(
        ProcessOrderPaymentRequest request,
        ISender sender,
        ICheckoutContext checkoutContext)
    {
        CheckoutSession checkoutSession = await checkoutContext.SessionManager.GetRequiredSessionAsync();
        ProcessOrderPaymentCommand command = new(checkoutSession.GetRequiredOrderId().Value, request.PaymentData);
        Result<OrderPaymentDto> result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }
}