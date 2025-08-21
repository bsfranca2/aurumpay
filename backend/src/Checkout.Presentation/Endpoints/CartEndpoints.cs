using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Create;
using AurumPay.Checkout.Presentation.Contracts;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.Endpoints;

public class CartEndpoints() : CarterModule("/cart")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/checkout", CheckoutWithPublicIds)
            .WithName(nameof(CheckoutWithPublicIds))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CheckoutWithPublicIds(
        CreateCheckoutRequest request,
        ISender sender)
    {
        CreateCheckoutSessionCommand command = new(request.CartItems);
        Result result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }
}