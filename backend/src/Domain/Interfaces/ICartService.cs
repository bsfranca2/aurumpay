using AurumPay.Domain.CheckoutSessions;

namespace AurumPay.Domain.Interfaces;

public interface ICartService
{
    Task<HashSet<CartItem>> GetCartItemsAsync(CancellationToken cancellationToken = default);
    Task SetCartItemsAsync(HashSet<CartItem> cartItems);
}