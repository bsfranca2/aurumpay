using Ardalis.Result;

using AurumPay.Domain.CheckoutSessions;

namespace AurumPay.Domain.Interfaces;

public interface ICartService
{
    Task<Result<HashSet<CartItem>>> GetCartItemsAsync(CancellationToken cancellationToken = default);
    Task SetCartItemsAsync(HashSet<CartItem> cartItems);
}