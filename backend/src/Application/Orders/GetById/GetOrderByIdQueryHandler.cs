using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Finalize;
using AurumPay.Application.SeedWork;
using AurumPay.Domain.Orders;

namespace AurumPay.Application.Orders.GetById;

internal sealed class GetOrderQueryHandler(
    IOrderRepository orderRepository
) : IQueryHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository.GetWithPaymentsAsync(new OrderId(request.OrderId));
        if (order is null)
        {
            return Result.Error("Order not found");
        }

        OrderDto orderDto = new(
            order.Id.Value,
            order.CustomerId.Value,
            order.StoreId.Value,
            order.OrderTotal,
            order.Status,
            order.PaymentStatus,
            order.CreatedAtUtc,
            order.OrderItems.Select(item => new OrderItemDto(
                item.ProductId.Value,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.TotalPrice
            )).ToList()
        );

        return Result.Success(orderDto);
    }
}