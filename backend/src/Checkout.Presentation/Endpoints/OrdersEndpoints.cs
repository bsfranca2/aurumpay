using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Finalize;
using AurumPay.Application.Orders.GetById;
using AurumPay.Application.Orders.ProcessOrderPayment;
using AurumPay.Checkout.Presentation.Contracts;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.Endpoints;

public class OrdersEndpoints : CarterModule
{
    public OrdersEndpoints() : base("/orders")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{orderId:long}", GetOrder)
            .WithName(nameof(GetOrder))
            .Produces<OrderDto>();
    }

    private static async Task<IResult> GetOrder(long orderId, ISender sender)
    {
        GetOrderByIdQuery query = new(orderId);
        Result<OrderDto> result = await sender.Send(query);
        return result.ToMinimalApiResult();
    }
}