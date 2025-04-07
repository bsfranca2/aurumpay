using AurumPay.Application.Common.Interfaces;
using AurumPay.Application.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Features.Checkout;

public class GetCheckout
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/checkout", Handler)
                .RequireAuthorization("ValidCheckoutSession")
                .WithTags("Checkout");
        }
    }

    private static async Task<IResult> Handler([FromServices] IHttpContextAccessor contextAccessor, AppDbContext dbContext)
    {
        var httpContext = contextAccessor.HttpContext!;
        var sessionId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;
        var sessionGuid = Guid.Parse(sessionId!);

        var session = await dbContext.CheckoutSessions.FirstOrDefaultAsync(cs => cs.Id == sessionGuid);
    
        return session != null 
            ? Results.Ok(session) 
            : Results.NotFound(new { code = "EMPTY_CART" });
    }
}