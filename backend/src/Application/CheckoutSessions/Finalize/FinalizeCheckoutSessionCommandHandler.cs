using Ardalis.Result;

using AurumPay.Application.Interfaces;
using AurumPay.Core;
using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Orders;

namespace AurumPay.Application.CheckoutSessions.Finalize;

internal sealed class FinalizeCheckoutCommandHandler(
    ICheckoutContext checkoutContext,
    ICheckoutSessionRepository checkoutSessionRepository,
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<FinalizeCheckoutSessionCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(FinalizeCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        CheckoutSession? session = await checkoutContext.SessionManager.GetCurrentSessionAsync();
        if (session is null)
        {
            return Result.Error("No active checkout session found");
        }

        if (session.CustomerId is null)
        {
            return Result.Error("Customer not identified");
        }

        if (session.CartItems.Count == 0)
        {
            return Result.Error("Cart is empty");
        }

        if (session.OrderId is not null)
        {
            Order? existingOrder = await orderRepository.GetByIdAsync(session.OrderId.Value);
            if (existingOrder != null)
            {
                return Result.Success(MapToOrderDto(existingOrder));
            }
        }

        await unitOfWork.BeginTransactionAsync();
        
        // TODO: Improve
        IEnumerable<Task<Product?>> productTasks = session.CartItems.Select(ci => productRepository.GetByIdAsync(ci.ProductId));
        Product?[] productsWithNull = await Task.WhenAll(productTasks);
        IEnumerable<Product> products = productsWithNull.Where(p => p != null).Select(p => p!);

        IEnumerable<OrderItem> orderItems =
            session.CartItems.Select(ci => OrderItem.Create(ci, products.First(p => p.Id == ci.ProductId)));

        Order order = Order.Create(
            session.StoreId,
            session.CustomerId.Value,
            session.Id,
            orderItems
        );

        await orderRepository.CreateAsync(order);

        session.CreateOrder(order.Id);
        await checkoutSessionRepository.UpdateAsync(session);

        await unitOfWork.CommitTransactionAsync();

        return Result.Success(MapToOrderDto(order));
    }

    private static OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto(
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
    }
}