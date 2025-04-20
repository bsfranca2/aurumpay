namespace AurumPay.Domain.Interfaces;

public interface ICheckoutContext
{
    IStoreContext Store { get; }
    ICartService CartService { get; }
    ICheckoutSessionManager SessionManager { get; }
    IDeviceIdentityProvider DeviceIdentity { get; }
}