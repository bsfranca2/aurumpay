using Ardalis.Result;
using Ardalis.Result.AspNetCore;

using AurumPay.Application.CheckoutSessions.Create;
using AurumPay.Application.CheckoutSessions.Get;
using AurumPay.Application.CheckoutSessions.UpdateCustomer;
using AurumPay.Checkout.Api.Common.Interfaces;

using MediatR;

namespace AurumPay.Checkout.Api.Presenter;

public class CheckoutEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/checkout/init/product", CheckoutInitProduct)
            .WithTags("Checkout");

        app.MapGet("/checkout/summary", CheckoutSummary)
            .WithTags("Checkout");

        app.MapPut("/checkout/customer", CheckoutCustomer)
            .WithTags("Checkout");
    }

    private static async Task<IResult> CheckoutInitProduct(CreateCheckoutSessionCommand command, ISender sender)
    {
        Result result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> CheckoutSummary(ISender sender)
    {
        Result<CheckoutSessionDto> result = await sender.Send(new GetCheckoutSessionQuery());
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> CheckoutCustomer(UpdateCheckoutSessionCustomerCommand command, ISender sender)
    {
        Result result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }
}