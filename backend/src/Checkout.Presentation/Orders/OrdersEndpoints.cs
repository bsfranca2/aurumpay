using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Finalize;
using AurumPay.Application.Orders.GetById;
using AurumPay.Application.Orders.ProcessOrderPayment;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.Orders;

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

        app.MapPost("/{orderId:long}/payment", ProcessOrderPayment)
            .WithName(nameof(ProcessOrderPayment))
            .Produces<OrderPaymentDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> GetOrder(long orderId, ISender sender)
    {
        GetOrderByIdQuery query = new(orderId);
        Result<OrderDto> result = await sender.Send(query);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> ProcessOrderPayment(
        long orderId,
        ProcessOrderPaymentRequest request,
        ISender sender)
    {
        ProcessOrderPaymentCommand command = new(orderId, request.PaymentData);
        Result<OrderPaymentDto> result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }
}