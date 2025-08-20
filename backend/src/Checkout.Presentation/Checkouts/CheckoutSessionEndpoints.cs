using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.AddCustomerAddress;
using AurumPay.Application.CheckoutSessions.Finalize;
using AurumPay.Application.CheckoutSessions.Get;
using AurumPay.Application.CheckoutSessions.SelectPaymentMethod;
using AurumPay.Application.CheckoutSessions.UpdateCustomer;
using AurumPay.Application.Customers;
using AurumPay.Domain.Payments.Methods;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.Checkouts;

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

        app.MapPost("/customer/addresses", AddCustomerAddress)
            .WithName(nameof(AddCustomerAddress))
            .Produces<CustomerAddressDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        app.MapPut("/payment-method", SelectCheckoutPaymentMethod)
            .WithName(nameof(SelectCheckoutPaymentMethod))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        app.MapPost("/finalize", FinalizeCheckoutSession)
            .WithName(nameof(FinalizeCheckoutSession))
            .Produces<OrderDto>()
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CheckoutSummary(ISender sender)
    {
        Result<CheckoutSessionDto> result = await sender.Send(new GetCheckoutSessionQuery());
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> CheckoutCustomer(IdentifyCustomerDto request, ISender sender)
    {
        UpdateCheckoutSessionCustomerCommand command = new(request.FullName, request.Email, request.PhoneNumber, request.Cpf);
        Result result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> AddCustomerAddress(AddCustomerAddressDto request, ISender sender)
    {
        AddCheckoutSessionCustomerAddressCommand command = new(request.Cep, request.AddressLine1, request.AddressLine2,
            request.Number, request.Neighborhood, request.City, request.State, request.Recipient);
        Result<CustomerAddressDto> result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> SelectCheckoutPaymentMethod(SelectPaymentMethodDto request, ISender sender)
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
}