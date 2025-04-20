using AurumPay.Domain.Interfaces;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class CheckoutContext(
    IStoreContext storeContext,
    ICartService cartService,
    ICheckoutSessionManager checkoutSessionManager,
    IDeviceIdentityProvider deviceIdentityProvider
) : ICheckoutContext
{
    public IStoreContext Store { get; } = storeContext;
    public ICartService CartService { get; } = cartService;
    public ICheckoutSessionManager SessionManager { get; } = checkoutSessionManager;
    public IDeviceIdentityProvider DeviceIdentity { get; } = deviceIdentityProvider;
}