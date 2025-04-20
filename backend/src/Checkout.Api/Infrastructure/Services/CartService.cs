using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class CartService : ICartService
{
    public Task<HashSet<CartItem>> GetCartItemsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HashSet<CartItem>());
    }

    public Task SetCartItemsAsync(HashSet<CartItem> cartItems)
    {
        return Task.CompletedTask;
    }
}