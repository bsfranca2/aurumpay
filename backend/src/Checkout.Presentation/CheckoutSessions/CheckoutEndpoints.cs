using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Create;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.CheckoutSessions;

public class CheckoutEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/checkout/init/product", CheckoutInitProduct)
            .WithName(nameof(CheckoutInitProduct))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CheckoutInitProduct(CreateCheckoutDto request, ISender sender)
    {
        CreateCheckoutSessionCommand command = new(request.CartItems);
        Result result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }
}