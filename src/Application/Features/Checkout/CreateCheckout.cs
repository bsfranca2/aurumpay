using System.Security.Claims;

using AurumPay.Application.Common.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AurumPay.Application.Features.Checkout;

public class CreateCheckout
{
    public record Request(List<CartItemDto> Items, string? PromoCode);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/checkout/init/product", Handler)
                .WithTags("Checkout");
        }
    }

    private static async Task<IResult> Handler([FromBody] Request request,
        [FromServices] IHttpContextAccessor contextAccessor, [FromServices] CheckoutSessionService sessionService)
    {
        HttpContext context = contextAccessor.HttpContext!;
        if (context.Items["StoreId"] is Guid storeId)
        {
            Guid checkoutSession = await sessionService.CreateNewSessionAsync(storeId, request.Items);
            await context.SignInAsync(
                new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim("SessionId", checkoutSession.ToString())], 
                    "CheckoutAuth")),
                new AuthenticationProperties 
                {
                    IsPersistent = true,
                    AllowRefresh = true
                });
            return Results.Ok(new { Id = checkoutSession });
        }

        return Results.BadRequest();
    }
}